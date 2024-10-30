using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Clients;
using Library.Core.Dto.Emails;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class EmailService : IEmailService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public EmailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> EmailExistsAsync(long id)
        {
            Email? email = await unitOfWork.EmailRepository.GetByIdAsync(id);

            return email != null;
        }

        public async Task<bool> EmailIsMainAsync(long id)
        {
            Email? email = await unitOfWork.EmailRepository.GetByIdAsync(id);

            if (email == null)
            {
                throw new NotFoundException($"Email with id {id} was not found!");
            }

            return email.IsMain;
        }

        public async Task<IEnumerable<EmailListDto>> GetClientEmailsAsync(long clientId)
        {
            List<EmailListDto> emails = mapper.Map<List<EmailListDto>>(await unitOfWork.EmailRepository.AllAsQueryable()
                .AsNoTracking()
                .Where(e => e.ClientId == clientId)
                .ToListAsync());

            return emails;
        }

        public EmailCreateDto CreateEmailCreateDto()
        {
            EmailCreateDto dto = new EmailCreateDto()
            {
                Type = EmailType.Personal
            };

            return dto;
        }

        public async Task<long> CreateEmailAsync(long clientId, EmailCreateDto dto)
        {
            Client? client = await unitOfWork.ClientRepository.AllAsQueryable()
                .Where(cl => cl.Id == clientId)
                .Include(cl => cl.Emails)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                throw new NotFoundException($"Client with id {clientId} was not found!");
            }

            Email newEmail = mapper.Map<Email>(dto);

            if (newEmail.IsMain)
            {
                await ChangeOldMainEmailAsync(clientId);
            }

            client.Emails.Add(newEmail);

            if (client.Emails.Count(e => e.IsMain) != 1)
            {
                throw new InvalidOperationException("Client must have exactly one main email!");
            }
            if (client.Emails.Count(e => e.Address.ToLower() == newEmail.Address.ToLower()) != 1)
            {
                throw new InvalidOperationException("Client cannot have duplicate emails!");
            }

            unitOfWork.ClientRepository.Update(client);
            await unitOfWork.SaveChangesAsync();

            return newEmail.Id;
        }

        public async Task ChangeClientMainEmailAsync(long clientId, long emailId)
        {
            List<Email> clientEmails = await unitOfWork.EmailRepository.AllAsQueryable()
                .Where(cl => cl.ClientId == clientId)
                .ToListAsync();

            Email? newMainEmail = clientEmails.FirstOrDefault(cl => cl.Id == emailId);

            if (newMainEmail == null)
            {
                throw new NotFoundException($"Email with id {emailId} was not found!");
            }
            if (newMainEmail.IsMain)
            {
                throw new InvalidOperationException("Email is already a main email!");
            }
            await ChangeOldMainEmailAsync(clientId);
            newMainEmail.IsMain = true;

            unitOfWork.EmailRepository.Update(newMainEmail);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<EmailEditDto> CreateEmailEditDtoAsync(long id)
        {
            EmailEditDto? dto = mapper.Map<EmailEditDto?>(await unitOfWork.EmailRepository.AllAsQueryable()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Email with id {id} was not found!");
            }

            return dto;
        }

        public async Task UpdateEmailAsync(long id, EmailEditDto dto)
        {
            Email? email = await unitOfWork.EmailRepository.AllAsQueryable()
                .Where(e => e.Id == id)
                .Include(e => e.Client)
                .ThenInclude(cl => cl.Emails)
                .FirstOrDefaultAsync();

            if (email == null)
            {
                throw new NotFoundException($"Email with id {id} was not found!");
            }

            bool changeOldMainEmail = !email.IsMain && dto.IsMain;
            if (changeOldMainEmail)
            {
                await ChangeOldMainEmailAsync(email.ClientId);
            }
            email.Address = dto.Address;
            email.IsMain = dto.IsMain;
            email.Type = dto.Type;
            unitOfWork.EmailRepository.Update(email);
            if (email.Client.Emails.Count(e => e.IsMain) != 1)
            {
                throw new InvalidOperationException("Client must have exactly one main email!");
            }
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<ClientEmailsEditDto> CreateClientEmailsEditDtoAsync(long clientId)
        {
            ClientEmailsEditDto? dto = mapper.Map<ClientEmailsEditDto?>(await unitOfWork.ClientRepository.AllAsQueryable()
                .AsNoTracking()
                .Where(cl => cl.Id == clientId)
                .Include(c => c.Emails)
                .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Client with id {clientId} was not found!");
            }

            return dto;
        }

        public async Task UpdateClientEmailsAsync(long clientId, ClientEmailsEditDto dto)
        {
            if (dto.Emails.Count(e => e.IsMain) != 1)
            {
                throw new ArgumentException("Client must have exactly one main email!");
            }

            Client? client = await unitOfWork.ClientRepository.AllAsQueryable()
                .Where(cl => cl.Id == clientId)
                .Include(cl => cl.Emails)
                .FirstOrDefaultAsync();

            if (client == null)
            {
                throw new NotFoundException($"Client with id {clientId} was not found!");
            }

            foreach (EmailCreateOrEditDto emailDto in dto.Emails)
            {
                if (emailDto.Id != null)
                {
                    Email? clientEmail = client.Emails.FirstOrDefault(a => a.Id == emailDto.Id);

                    if (clientEmail == null)
                    {
                        throw new NotFoundException($"Email with {emailDto.Id} was not found!");
                    }

                    clientEmail.Address = emailDto.Address;
                    clientEmail.Type = emailDto.Type;
                    clientEmail.IsMain = emailDto.IsMain;
                }
                else
                {
                    Email? email = new Email()
                    {
                        Address = emailDto.Address,
                        Type = emailDto.Type,
                        IsMain = emailDto.IsMain,
                    };

                    client.Emails.Add(email);
                }
            }

            IEnumerable<Email> archivedEmails = client.Emails
                .Where(a => !dto.Emails.Select(da => da.Id).Contains(a.Id));

            foreach (Email email in archivedEmails)
            {
                email.Status = EmailStatus.Archived;
            }

            unitOfWork.ClientRepository.Update(client);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task ChangeOldMainEmailAsync(long clientId)
        {
            List<Email> clientEmails = await unitOfWork.EmailRepository.AllAsQueryable()
            .Where(cl => cl.ClientId == clientId)
            .ToListAsync();

            Email? oldMainEmail = clientEmails.FirstOrDefault(cl => cl.IsMain);

            if (oldMainEmail == null)
            {
                throw new InvalidOperationException("Client must have one residency address!");
            }

            oldMainEmail.IsMain = false;
            unitOfWork.EmailRepository.Update(oldMainEmail);
        }

        public async Task ArchiveEmailAsync(long id)
        {
            Email? email = await unitOfWork.EmailRepository.GetByIdAsync(id);

            if (email == null)
            {
                throw new NotFoundException($"Email with id {id} was not found!");
            }

            if (email.IsMain)
            {
                throw new InvalidOperationException("Cannot archive main email!");
            }

            email.Status = EmailStatus.Archived;
            unitOfWork.EmailRepository.Update(email);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
