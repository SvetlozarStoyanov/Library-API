using Library.Core.Dto.Clients;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;

namespace Library.Core.Contracts.DbServices
{
    public interface IClientService
    {
        /// <summary>
        /// Returns <see cref="true"/> if <see cref="Client"/> with given <paramref name="id"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ClientExistsAsync(long id);

        /// <summary>
        /// Checks if <see cref="Client"/> has <see cref="Client.Fines"/>
        /// with <see cref="Fine.Status"/> as <see cref="FineStatus.Unpaid"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ClientHasUnpaidFinesAsync(long id);

        /// <summary>
        /// Returns all clients
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="ClientListDto"/></returns>
        Task<IEnumerable<ClientListDto>> GetAllClientsAsync();

        /// <summary>
        /// Returns a <see cref="ClientDetailsDto"/> of <see cref="Client"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ClientDetailsDto"/></returns>
        Task<ClientDetailsDto> GetClientByIdAsync(long id);

        /// <summary>
        /// Creates a <see cref="ClientCreateDto"/> and returns it
        /// </summary>
        /// <returns><see cref="ClientCreateDto"/> dto</returns>
        ClientCreateDto CreateClientCreateDtoAsync();

        /// <summary>
        /// Creates a <see cref="Client"/> from given <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="int"/> clientId</returns>
        Task<long> CreateClientAsync(ClientCreateDto dto);

        /// <summary>
        /// Creates <see cref="ClientCreateDto"/> from <see cref="Client"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ClientCreateDto"/> dto</returns>
        Task<ClientEditDto> CreateClientEditDtoAsync(long id);

        /// <summary>
        /// Updates a <see cref="Client"/> with given <paramref name="id"/> 
        /// with given data from <paramref name="dto"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateClientAsync(long id, ClientEditDto dto);

        /// <summary>
        /// Deletes <see cref="Client"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteClientByIdAsync(long id);
    }
}
