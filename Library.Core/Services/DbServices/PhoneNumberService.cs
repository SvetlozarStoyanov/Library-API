using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Clients;
using Library.Core.Dto.PhoneNumbers;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PhoneNumberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> PhoneNumberExistsAsync(long id)
        {
            PhoneNumber? phoneNumber = await unitOfWork.PhoneNumberRepository.GetByIdAsync(id);

            return phoneNumber != null;
        }

        public async Task<bool> PhoneNumberIsMainAsync(long id)
        {
            PhoneNumber? phoneNumber = await unitOfWork.PhoneNumberRepository.GetByIdAsync(id);

            if (phoneNumber == null)
            {
                throw new NotFoundException($"Phone number with id {id} was not found!");
            }

            return phoneNumber.IsMain;
        }


        public async Task<IEnumerable<PhoneNumberListDto>> GetClientPhoneNumbersAsync(long clientId)
        {
            List<PhoneNumberListDto> phoneNumbers = mapper.Map<List<PhoneNumberListDto>>(await unitOfWork.PhoneNumberRepository
                .AllReadOnlyAsync(pn => pn.ClientId == clientId));

            return phoneNumbers;
        }

        public PhoneNumberCreateDto CreatePhoneNumberCreateDto()
        {
            PhoneNumberCreateDto dto = new PhoneNumberCreateDto()
            {
                Type = PhoneNumberType.Mobile
            };

            return dto;
        }

        public async Task<long> CreatePhoneNumberAsync(long clientId, PhoneNumberCreateDto dto)
        {
            Client? client = await unitOfWork.ClientRepository.AllAsQueryable()
                .Where(cl => cl.Id == clientId)
                .Include(cl => cl.PhoneNumbers)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                throw new NotFoundException($"Client with id {clientId} was not found!");
            }

            PhoneNumber newPhoneNumber = mapper.Map<PhoneNumber>(dto);

            if (newPhoneNumber.IsMain)
            {
                await ChangeOldMainPhoneNumberAsync(clientId);
            }

            client.PhoneNumbers.Add(newPhoneNumber);
            await unitOfWork.SaveChangesAsync();
            return newPhoneNumber.Id;
        }

        public async Task<PhoneNumberEditDto> CreatePhoneNumberEditDtoAsync(long id)
        {
            PhoneNumberEditDto? dto = mapper.Map<PhoneNumberEditDto?>(await unitOfWork.PhoneNumberRepository
                .GetByIdAsync(id));

            if (dto == null)
            {
                throw new NotFoundException($"Phone number with id {id} was not found!");
            }

            return dto;
        }

        public async Task UpdatePhoneNumberAsync(long id, PhoneNumberEditDto dto)
        {
            PhoneNumber? phoneNumber = await unitOfWork.PhoneNumberRepository.AllAsQueryable()
                .Where(e => e.Id == id)
                .Include(e => e.Client)
                .ThenInclude(cl => cl.PhoneNumbers)
                .FirstOrDefaultAsync();

            if (phoneNumber == null)
            {
                throw new NotFoundException($"Phone number with id {id} was not found!");
            }

            bool changeOldMainPhoneNumber = !phoneNumber.IsMain && dto.IsMain;
            if (changeOldMainPhoneNumber)
            {
                await ChangeOldMainPhoneNumberAsync(phoneNumber.ClientId);
            }
            phoneNumber.Number = dto.Number;
            phoneNumber.IsMain = dto.IsMain;
            phoneNumber.CountryId = dto.CountryId;
            phoneNumber.Type = dto.Type;
            unitOfWork.PhoneNumberRepository.Update(phoneNumber);

            if (phoneNumber.Client.PhoneNumbers.Count(e => e.IsMain) != 1)
            {
                throw new InvalidOperationException("Client must have exactly one phone number!");
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task ChangeClientMainPhoneNumberAsync(long clientId, long phoneNumberId)
        {
            List<PhoneNumber> clientEmails = await unitOfWork.PhoneNumberRepository.AllAsQueryable()
                .Where(cl => cl.ClientId == clientId)
                .ToListAsync();

            PhoneNumber? newMainPhoneNumber = clientEmails.FirstOrDefault(cl => cl.Id == phoneNumberId);

            if (newMainPhoneNumber == null)
            {
                throw new NotFoundException($"Phone number with id {phoneNumberId} was not found!");
            }
            if (newMainPhoneNumber.IsMain)
            {
                throw new InvalidOperationException("Phone number is already a main phone number!");
            }
            await ChangeOldMainPhoneNumberAsync(clientId);
            newMainPhoneNumber.IsMain = true;

            unitOfWork.PhoneNumberRepository.Update(newMainPhoneNumber);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<ClientPhoneNumbersEditDto> CreateClientEditPhoneNumbersDtoAsync(long clientId)
        {
            ClientPhoneNumbersEditDto? dto = mapper.Map<ClientPhoneNumbersEditDto?>(await unitOfWork.ClientRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(cl => cl.Id == clientId)
                .Include(cl => cl.PhoneNumbers)
                .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Client with id {clientId} was not found!");
            }

            return dto;
        }

        public async Task UpdateClientPhoneNumbersAsync(long clientId, ClientPhoneNumbersEditDto dto)
        {
            if (dto.PhoneNumbers.Count(pn => pn.IsMain) != 1)
            {
                throw new ArgumentException("Client must have exactly one main phone number!");
            }

            Client? client = await unitOfWork.ClientRepository.AllAsQueryable()
                .Where(cl => cl.Id == clientId)
                .Include(cl => cl.PhoneNumbers)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                throw new NotFoundException($"Client with {clientId} was not found!");
            }

            foreach (PhoneNumberCreateOrEditDto phoneNumberDto in dto.PhoneNumbers)
            {
                if (phoneNumberDto.Id != null)
                {
                    PhoneNumber? clientPhoneNumber = client.PhoneNumbers.FirstOrDefault(a => a.Id == phoneNumberDto.Id);

                    if (clientPhoneNumber == null)
                    {
                        throw new NotFoundException($"Phone number with {phoneNumberDto.Id} was not found!");
                    }

                    clientPhoneNumber.Number = phoneNumberDto.Number;
                    clientPhoneNumber.Type = phoneNumberDto.Type;
                    clientPhoneNumber.IsMain = phoneNumberDto.IsMain;
                    clientPhoneNumber.CountryId = phoneNumberDto.CountryId;

                }
                else
                {
                    PhoneNumber? phoneNumber = new PhoneNumber()
                    {
                        CountryId = phoneNumberDto.CountryId,
                        Number = phoneNumberDto.Number,
                        Type = phoneNumberDto.Type,
                        IsMain = phoneNumberDto.IsMain
                    };

                    client.PhoneNumbers.Add(phoneNumber);
                }
            }

            IEnumerable<PhoneNumber> archivedPhoneNumbers = client.PhoneNumbers
                .Where(a => !dto.PhoneNumbers.Select(da => da.Id).Contains(a.Id));

            foreach (PhoneNumber phoneNumber in archivedPhoneNumbers)
            {
                phoneNumber.Status = PhoneNumberStatus.Archived;
            }

            unitOfWork.ClientRepository.Update(client);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task ChangeOldMainPhoneNumberAsync(long clientId)
        {
            List<PhoneNumber> clientPhoneNumbers = await unitOfWork.PhoneNumberRepository.AllAsQueryable()
                .Where(cl => cl.ClientId == clientId)
                .ToListAsync();

            PhoneNumber? oldMainPhoneNumber = clientPhoneNumbers.FirstOrDefault(cl => cl.IsMain);

            if (oldMainPhoneNumber == null)
            {
                throw new InvalidOperationException("Client must have one main phone number!");
            }

            oldMainPhoneNumber.IsMain = false;
            unitOfWork.PhoneNumberRepository.Update(oldMainPhoneNumber);
        }

        public async Task ArchivePhoneNumberAsync(long id)
        {
            PhoneNumber? phoneNumber = await unitOfWork.PhoneNumberRepository.GetByIdAsync(id);

            if (phoneNumber == null)
            {
                throw new NotFoundException($"Phone number with id {id} was not found!");
            }

            if (phoneNumber.IsMain)
            {
                throw new InvalidOperationException("Cannot archive main phone number!");
            }

            phoneNumber.Status = PhoneNumberStatus.Archived;
            unitOfWork.PhoneNumberRepository.Update(phoneNumber);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
