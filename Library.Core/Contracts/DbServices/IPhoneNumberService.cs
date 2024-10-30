using Library.Core.Dto.Clients;
using Library.Core.Dto.PhoneNumbers;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;

namespace Library.Core.Contracts.DbServices
{
    public interface IPhoneNumberService
    {
        /// <summary>
        /// Checks if <see cref="PhoneNumber"/> with <paramref name="id"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> PhoneNumberExistsAsync(long id);

        /// <summary>
        /// Checks if <see cref="PhoneNumber"/> with <paramref name="id"/>
        /// has <see cref="PhoneNumber.IsMain"/> equal to true
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> isMain</returns>
        Task<bool> PhoneNumberIsMainAsync(long id);

        /// <summary>
        /// Returns <see cref="Client.PhoneNumbers"/> of <see cref="Client"/>
        /// with <paramref name="clientId"/> as <see cref="IEnumerable{T}"/> of <see cref="PhoneNumberListDto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="PhoneNumberListDto"/></returns>
        Task<IEnumerable<PhoneNumberListDto>> GetClientPhoneNumbersAsync(long clientId);

        /// <summary>
        /// Creates a <see cref="PhoneNumberCreateDto"/>
        /// </summary>
        /// <returns><see cref="PhoneNumberCreateDto"/> dto</returns>
        PhoneNumberCreateDto CreatePhoneNumberCreateDto();

        /// <summary> 
        /// Creates a <see cref="PhoneNumber"/> with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dto"></param>
        /// <returns><see cref="PhoneNumber.Id"/></returns>
        Task<long> CreatePhoneNumberAsync(long clientId, PhoneNumberCreateDto dto);

        /// <summary>
        /// Changes a <see cref="PhoneNumber"/> with <paramref name="phoneNumberId"/>
        /// <see cref="PhoneNumber.IsMain"/> to true. Also changes the old main <see cref="PhoneNumber"/>
        /// <see cref="PhoneNumber.IsMain"/> to false
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="phoneNumberId"></param>
        /// <returns></returns>
        Task ChangeClientMainPhoneNumberAsync(long clientId, long phoneNumberId);

        /// <summary>
        /// Creates a <see cref="PhoneNumberEditDto"/> for <see cref="PhoneNumber"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="PhoneNumberEditDto"/> dto</returns>
        Task<PhoneNumberEditDto> CreatePhoneNumberEditDtoAsync(long id);

        /// <summary>
        /// Updates <see cref="PhoneNumber"/> with <paramref name="id"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdatePhoneNumberAsync(long id, PhoneNumberEditDto dto);

        /// <summary>
        /// Creates a <see cref="ClientPhoneNumbersEditDto"/> of <see cref="Client"/>
        /// with <paramref name="clientId"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns><see cref="ClientPhoneNumbersEditDto"/> dto</returns>
        Task<ClientPhoneNumbersEditDto> CreateClientEditPhoneNumbersDtoAsync(long clientId);

        /// <summary>
        /// Updates <see cref="Client.PhoneNumbers"/> of <see cref="Client"/> with <paramref name="clientId"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateClientPhoneNumbersAsync(long clientId, ClientPhoneNumbersEditDto dto);

        /// <summary>
        /// Sets <see cref="PhoneNumber.Status"/> to <see cref="PhoneNumberStatus.Archived"/>
        /// for <see cref="Email"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ArchivePhoneNumberAsync(long id);
    }
}
