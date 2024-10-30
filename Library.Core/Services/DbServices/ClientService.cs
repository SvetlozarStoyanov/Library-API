using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Addresses;
using Library.Core.Dto.Clients;
using Library.Core.Dto.Emails;
using Library.Core.Dto.PhoneNumbers;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace Library.Core.Services.DbServices
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ClientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> ClientExistsAsync(long id)
        {
            Client? client = await unitOfWork.ClientRepository.GetByIdAsync(id);

            return client != null;
        }

        public async Task<bool> ClientHasUnpaidFinesAsync(long id)
        {
            IEnumerable<Fine>? clientFines = await unitOfWork.ClientRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(cl => cl.Id == id)
                .Select(cl => cl.ClientCards.SelectMany(ch => ch.Checkouts.SelectMany(ch => ch.Fines.Where(f => f.Status == FineStatus.Unpaid))))
                .FirstOrDefaultAsync();

            if (clientFines == null)
            {
                throw new NotFoundException("Client does not exist!");
            }

            return clientFines.Any(f => f.Status == FineStatus.Unpaid);
        }

        public async Task<IEnumerable<ClientListDto>> GetAllClientsAsync()
        {
            List<ClientListDto> clients = mapper.Map<List<ClientListDto>>(await unitOfWork.ClientRepository
                .AllReadOnlyAsync());

            return clients;
        }

        public async Task<ClientDetailsDto> GetClientByIdAsync(long id)
        {
            ClientDetailsDto? client = mapper.Map<ClientDetailsDto?>(await unitOfWork.ClientRepository
                .AllAsQueryable(cl => cl.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync());

            if (client == null)
            {
                throw new NotFoundException($"Client with id {id} was not found!");
            }

            return client;
        }

        public ClientCreateDto CreateClientCreateDtoAsync()
        {
            ClientCreateDto dto = new ClientCreateDto()
            {
                DateOfBirth = DateTime.Now,
                Addresses = new HashSet<AddressCreateDto>()
                {
                    new AddressCreateDto()
                    {
                        City = null,
                        CountryId = 0,
                        AddressLine = null,
                        PostalCode = null,
                        Type = AddressType.Residency
                    }
                },
                Emails = new HashSet<EmailCreateDto>()
                {
                    new EmailCreateDto()
                    {
                        Address = null,
                        Type = EmailType.Personal,
                        IsMain = true
                    }
                },
                PhoneNumbers = new HashSet<PhoneNumberCreateDto>()
                {
                    new PhoneNumberCreateDto()
                    {
                        Number = null,
                        Type = PhoneNumberType.Mobile,
                        IsMain = true
                    }
                }
            };

            return dto;
        }

        public async Task<long> CreateClientAsync(ClientCreateDto dto)
        {
            if (dto.UnifiedCivilNumber != null)
            {
                if (await ClientWithUnifiedCivilNumberExistsAsync(dto.UnifiedCivilNumber, null))
                {
                    throw new InvalidOperationException("Client with such Unified Civil Number already exists!");
                }
                if (!UnifiedCivilNumberAndDateOfBirthMatch(dto.UnifiedCivilNumber, dto.DateOfBirth))
                {
                    throw new ArgumentException("Unified Civil Number birth date and Date Of Birth do not match!");
                }
            }

            if (dto.Addresses == null || dto.Addresses.Count(a => a.Type == AddressType.Residency) != 1)
            {
                throw new ArgumentException("Client must be created with exactly 1 main address!");
            }
            if (dto.Emails == null || dto.Emails.Count(a => a.IsMain) != 1)
            {
                throw new ArgumentException("Client must be created with exactly 1 main email address!");
            }
            if (dto.PhoneNumbers == null || dto.PhoneNumbers.Count(a => a.IsMain) != 1)
            {
                throw new ArgumentException("Client must be created with exactly 1 main phone number!");
            }

            Client client = mapper.Map<Client>(dto);
            AddAdressesToClient(dto.Addresses, client.Addresses);
            AddPhoneNumbersToClient(dto.PhoneNumbers, client.PhoneNumbers);
            AddEmailsToClient(dto.Emails, client.Emails);
            client.Code = await GenerateClientCodeAsync();
            await unitOfWork.ClientRepository.AddAsync(client);
            await unitOfWork.SaveChangesAsync();

            return client.Id;
        }

        public async Task<ClientEditDto> CreateClientEditDtoAsync(long id)
        {
            ClientEditDto? dto = mapper.Map<ClientEditDto?>(await unitOfWork.ClientRepository.AllAsQueryable().AsNoTracking()
                .Where(cl => cl.Id == id)
                .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Client with id {id} was not found!");
            }

            return dto;
        }

        public async Task UpdateClientAsync(long id, ClientEditDto dto)
        {
            Client? client = await unitOfWork.ClientRepository.GetByIdAsync(id);

            if (client == null)
            {
                throw new NotFoundException($"Client with id {id} was not found!");
            }

            if (dto.UnifiedCivilNumber != null)
            {
                if (await ClientWithUnifiedCivilNumberExistsAsync(dto.UnifiedCivilNumber, id))
                {
                    throw new InvalidOperationException("Client with such Unified Civil Number already exists!");
                }
            }

            client.FirstName = dto.FirstName;
            client.MiddleName = dto.MiddleName;
            client.LastName = dto.LastName;
            client.UnifiedCivilNumber = dto.UnifiedCivilNumber;
            client.DateOfBirth = dto.DateOfBirth;
            unitOfWork.ClientRepository.Update(client);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteClientByIdAsync(long id)
        {
            Client? client = await unitOfWork.ClientRepository.AllAsQueryable()
                .FirstOrDefaultAsync(cl => cl.Id == id);

            if (client == null)
            {
                throw new NotFoundException($"Client with id {id} was not found!");
            }

            await unitOfWork.ClientRepository.DeleteAsync(client.Id);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task<bool> ClientWithUnifiedCivilNumberExistsAsync(string unifiedCivilNumber, long? id)
        {
            if (id == null)
            {
                return await unitOfWork.ClientRepository.AllAsQueryable().AsNoTracking().AnyAsync(c => c.UnifiedCivilNumber == unifiedCivilNumber);
            }
            return await unitOfWork.ClientRepository.AllAsQueryable().AsNoTracking().AnyAsync(c => c.UnifiedCivilNumber == unifiedCivilNumber && c.Id != id);
        }

        private bool UnifiedCivilNumberAndDateOfBirthMatch(string unifiedCivilNumber, DateTime dateOfBirth)
        {
            Regex regex = new Regex(@"(?<year>[0-9]{2})((?<month>(0[0-9]{1}|1[0-2]{1})|(4[1-9]|5[0-2])))(?<day>(0[1-9]{1}|1[1-9]{1}|2[0-9]{1}|3[0-1]{1}))(?<number>[0-9]{4})");
            Match match = regex.Match(unifiedCivilNumber);

            int year = int.Parse(match.Groups["year"].ToString());
            int month = int.Parse(match.Groups["month"].ToString());
            int day = int.Parse(match.Groups["day"].ToString());
            year += 1900;
            if (dateOfBirth.Year > 1999)
            {
                month -= 40;
                year += 100;
            }

            return year == dateOfBirth.Year && month == dateOfBirth.Month && day == dateOfBirth.Day;
        }

        private void AddAdressesToClient(IEnumerable<AddressCreateDto> dtoAddresses,
            ICollection<Address> clientAddreses)
        {
            foreach (AddressCreateDto address in dtoAddresses)
            {
                if (dtoAddresses.Count(a => a.City.ToLower() == address.City.ToLower()
                    && a.AddressLine.ToLower() == address.AddressLine.ToLower()) > 1)
                {
                    throw new ArgumentException("Cannot add duplicate addresses to one client!");
                }
                Address newAddress = mapper.Map<Address>(address);
                clientAddreses.Add(newAddress);
            }
        }

        private void AddPhoneNumbersToClient(IEnumerable<PhoneNumberCreateDto> dtoPhoneNumbers,
            ICollection<PhoneNumber> clientPhoneNumbers)
        {
            foreach (PhoneNumberCreateDto phoneNumber in dtoPhoneNumbers)
            {
                if (dtoPhoneNumbers.Count(pn => pn.Number == phoneNumber.Number) > 1)
                {
                    throw new ArgumentException("Cannot add duplicate phone numbers to one client!");
                }
                PhoneNumber newPhoneNumber = mapper.Map<PhoneNumber>(phoneNumber);
                clientPhoneNumbers.Add(newPhoneNumber);
            }
        }

        private void AddEmailsToClient(IEnumerable<EmailCreateDto> dtoEmails, ICollection<Email> clientEmails)
        {
            foreach (EmailCreateDto email in dtoEmails)
            {
                if (dtoEmails.Count(e => e.Address == email.Address) > 1)
                {
                    throw new ArgumentException("Cannot add duplicate phone emails to one client!");
                }
                Email newEmail = mapper.Map<Email>(email);
                clientEmails.Add(newEmail);
            }
        }

        private async Task<string> GenerateClientCodeAsync()
        {
            List<string> clientCodes = await unitOfWork.ClientRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Select(cl => cl.Code)
                .ToListAsync();

            Random random = new Random();
            char letterOne = (char)random.Next(65, 91);
            char letterTwo = (char)random.Next(65, 91);
            StringBuilder sb = new StringBuilder();
            sb.Append($"{letterOne}{letterTwo}-{random.Next(100000, 1000000)}");

            while (clientCodes.Contains(sb.ToString()))
            {
                sb.Clear();
                sb.Append($"{letterOne}{letterTwo}-{random.Next(100000, 1000000)}");
            }

            return sb.ToString();
        }
    }
}
