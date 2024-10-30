using Library.Core.Dto.ClientCards;
using Library.Core.Dto.ClientCardStatusChanges;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;

namespace Library.Core.Contracts.DbServices
{
    public interface IClientCardService
    {
        /// <summary>
        /// Checks if <see cref="ClientCard"/> with given <paramref name="id"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><paramref name="id"/></returns>
        Task<bool> ClientCardExistsAsync(long id);

        /// <summary>
        /// Checks if <see cref="ClientCard"/> with <paramref name="id"/> can be reactivated
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> canBeActivated</returns>
        Task<bool> CanReactivateClientCardAsync(long id);

        /// <summary>
        /// Checks if <see cref="Client"/> has an active <see cref="Client.ClientCards"/>
        /// with same type as <paramref name="clientCardTypeId"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientCardTypeId"></param>
        /// <returns><see cref="bool"/> result</returns>
        Task<bool> ClientHasSameTypeOfCardAsync(long clientId, long clientCardTypeId);

        /// <summary>
        /// Checks if <see cref="ClientCard"/>  has unfinalized <see cref="ClientCard.Checkouts"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> result</returns>
        Task<bool> ClientCardHasUnfinalizedCheckoutsAsync(long id);

        /// <summary>
        /// Checks if <see cref="ClientCard"/> is expired.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> canBeRenewed</returns>
        Task<bool> ClientCardIsExpiredAsync(long id);

        /// <summary>
        /// Checks if <see cref="ClientCard"/> is eligible to checkout books
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> canCheckout</returns>
        Task<bool> ClientCardCanCreateCheckoutsAsync(long id);

        /// <summary>
        /// Returns the available quantity a <see cref="ClientCard"/> has
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="int"/> result</returns>
        Task<int> GetClientCardAvailableCheckoutQuantityAsync(long id);

        /// <summary>
        /// Returns the <see cref="ClientCard.ClientId"/> of <see cref="ClientCard"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ClientCard.ClientId"/></returns>
        Task<long> GetClientCardClientIdAsync(long id);

        /// <summary>
        /// Returns the <see cref="Client.ClientCards"/> of <see cref="Client"/> with <paramref name="clientId"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="ClientCardListDto"/></returns>
        Task<IEnumerable<ClientCardListDto>> GetClientCardsByClientIdAsync(long clientId);

        /// <summary>
        /// Creates a <see cref="ClientCardCreateDto"/> for <see cref="Client"/> witg given <paramref name="clientId"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ClientCardCreateDto CreateClientCardCreateDto(long clientId);

        /// <summary>
        /// Creates a <see cref="ClientCard"/> with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dto"></param>
        /// <returns><see cref="ClientCard.Id"/></returns>
        Task<long> CreateCreditClientCardAsync(long clientId, ClientCardCreateDto dto);

        /// <summary>
        /// Creates a <see cref="ClientCard.ClientCardType"/> time limit 
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="int"/> timeLimit</returns>
        Task<int> GetClientCardCheckoutTimeLimitAsync(long id);

        /// <summary>
        /// Creates a <see cref="ClientCardReactivateDto"/> and returns it
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="ClientCardReactivateDto"/> dto</returns>
        Task<ClientCardReactivateDto> CreateClientCardReactivateDtoAsync(long id);

        /// <summary>
        /// Reactivates a <see cref="ClientCard"/> with given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ReactivateClientCardAsync(long id);

        /// <summary>
        /// Reactivates a <see cref="ClientCard"/> with given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></return
        Task DeactivateClientCardAsync(long id);

        /// <summary>
        /// Renews <see cref="ClientCard"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RenewClientCardAsync(long id);

        /// <summary>
        /// Changes the <see cref="ClientCard.Status"/> to <see cref="ClientCardStatus.Lost"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task LoseClientCardAsync(long id);

        /// <summary>
        /// Changes lost <see cref="ClientCard"/>'s, with given <paramref name="id"/>,
        /// <see cref="ClientCard.Status"/> to <see cref="ClientCardStatus.Active"/> or <see cref="ClientCardStatus"/>
        /// depending on <see cref="ClientCardStatusChangeRecoveryDto.ReactivateClientCard"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task RecoverClientCardAsync(long id, ClientCardStatusChangeRecoveryDto dto);

        /// <summary>
        /// Changes the <see cref="ClientCard.Status"/> to <see cref="ClientCardStatus.Suspended"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SuspendClientCardAsync(long id);

        /// <summary>
        /// Changes the <see cref="ClientCard.Status"/> to <see cref="ClientCardStatus.Active"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UnsuspendClientCardAsync(long id);
    }
}
