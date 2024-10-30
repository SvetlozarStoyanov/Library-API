using Library.Core.Dto.Clients;
using Library.Core.Dto.Emails;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;

namespace Library.Core.Contracts.DbServices
{
    public interface IEmailService
    {
        /// <summary>
        /// Checks if <see cref="Email"/> with <paramref name="id"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> EmailExistsAsync(long id);

        /// <summary>
        /// Checks if <see cref="Email"/> with <paramref name="id"/>
        /// has <see cref="Email.IsMain"/> equal to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> isMain</returns>
        Task<bool> EmailIsMainAsync(long id);

        /// <summary>
        /// Returns <see cref="Client.Emails"/> of <see cref="Client"/>
        /// with <paramref name="clientId"/> as <see cref="IEnumerable{T}"/> of <see cref="EmailListDto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<IEnumerable<EmailListDto>> GetClientEmailsAsync(long clientId);

        /// <summary>
        /// Creates an <see cref="EmailCreateDto"/>
        /// </summary>
        /// <returns><see cref="EmailCreateDto"/> dto</returns>
        EmailCreateDto CreateEmailCreateDto();

        /// <summary>
        /// Creates an <see cref="Email"/> with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dto"></param>
        /// <returns><see cref="Email.Id"/></returns>
        Task<long> CreateEmailAsync(long clientId, EmailCreateDto dto);

        /// <summary>
        /// Creates an <see cref="EmailEditDto"/> for <see cref="Email"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="EmailEditDto"/> dto</returns>
        Task<EmailEditDto> CreateEmailEditDtoAsync(long id);

        /// <summary>
        /// Updates <see cref="Email"/> with <paramref name="id"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateEmailAsync(long id, EmailEditDto dto);

        /// <summary>
        /// Changes a <see cref="Email"/> with <paramref name="emailId"/>
        /// <see cref="Email.IsMain"/> to true. Also changes the old main <see cref="Email"/>
        /// <see cref="Email.IsMain"/> to false
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="phoneNumberId"></param>
        /// <returns></returns>
        Task ChangeClientMainEmailAsync(long clientId, long emailId);

        /// <summary>
        /// Creates a <see cref="ClientEmailsEditDto"/> of <see cref="Client"/>
        /// with <paramref name="clientId"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns><see cref="ClientEmailsEditDto"/> dto</returns>
        Task<ClientEmailsEditDto> CreateClientEmailsEditDtoAsync(long clientId);

        /// <summary>
        /// Updates the <see cref="Client.Emails"/> of <see cref="Client"/>
        /// with given <paramref name="clientId"/> with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateClientEmailsAsync(long clientId, ClientEmailsEditDto dto);

        /// <summary>
        /// Sets <see cref="Email.Status"/> to <see cref="EmailStatus.Archived"/>
        /// for <see cref="Email"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ArchiveEmailAsync(long id);
    }
}
