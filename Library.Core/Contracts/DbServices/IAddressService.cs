using Library.Core.Dto.Addresses;
using Library.Core.Dto.Clients;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;

namespace Library.Core.Contracts.DbServices
{
    public interface IAddressService
    {
        /// <summary>
        /// Checks if <see cref="Address"/>
        /// with given <paramref name="id"/> is a main <see cref="Address"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> AddressIsResidencyAsync(long id);

        /// <summary>
        /// Checks if <see cref="Address"/> with <paramref name="id"/>
        /// exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> AddressExistsAsync(long id);

        /// <summary>
        /// Returns the <see cref="Client.Addresses"/> for <see cref="Client"/>
        /// with given <paramref name="id"/> as <see cref="IEnumerable{T}"/> of <see cref="AddressListDto"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="AddressListDto"/></returns>
        Task<IEnumerable<AddressListDto>> GetClientAddressesAsync(long id);

        /// <summary>
        /// Creates an <see cref="AddressCreateDto"/>
        /// </summary>
        /// <returns><see cref="AddressCreateDto"/> dto</returns>
        AddressCreateDto CreateAddressCreateDtoAsync();

        /// <summary>
        /// Creates an <see cref="Address"/> for <see cref="Client"/> with <paramref name="clientId"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dto"></param>
        /// <returns><see cref="Address.Id"/></returns>
        Task<long> CreateAddressAsync(long clientId, AddressCreateDto dto);

        /// <summary>
        /// Creates an <see cref="AddressEditDto"/> for <see cref="Address"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AddressEditDto"/> dto</returns>
        Task<AddressEditDto> CreateAddressEditDtoAsync(long id);

        /// <summary>
        /// Updates <see cref="Address"/> with <paramref name="id"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateAddressAsync(long id, AddressEditDto dto);

        /// <summary>
        /// Creates a <see cref="ClientAddressesEditDto"/> for <see cref="Client.Addresses"/>
        /// with given <paramref name="clientId"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns><see cref="ClientAddressesEditDto"/> dto</returns>
        Task<ClientAddressesEditDto> CreateClientAddressesEditDtoAsync(long clientId);

        /// <summary>
        /// Updates <see cref="Client.Addresses"/> for <see cref="Client"/> 
        /// with given <paramref name="clientId"/> with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateClientAddressesAlternateAsync(long clientId, ClientAddressesEditDto dto);

        /// <summary>
        /// Changes the <see cref="Address.Type"/> to <see cref="AddressType.Residency"/>
        /// of <see cref="Address"/> with <paramref name="addressId"/>. Changes the previous 
        /// <see cref="Address"/> of <see cref="Client"/> with <paramref name="clientId"/>
        /// to <see cref="AddressType.Secondary"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="addressId"></param>
        /// <returns></returns>
        Task ChangeClientResidencyAddressAsync(long clientId, long addressId);

        /// <summary>
        /// Sets <see cref="Address.Status"/> to <see cref="AddressStatus.Archived"/>
        /// for <see cref="Address"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ArchiveAddressAsync(long id);

        /// <summary>
        /// Deletes <see cref="Address"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAddressAsync(long id);
    }
}
