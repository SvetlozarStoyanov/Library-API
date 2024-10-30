using Library.Core.Dto.ClientCardStatusChanges;
using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface IClientCardStatusChangeService
    {
        /// <summary>
        /// Creates a <see cref="ClientCardStatusChangeRecoveryDto"/> for <see cref="ClientCard"/> with
        /// <paramref name="clientCardId"/>
        /// </summary>
        /// <param name="clientCardId"></param>
        /// <returns><see cref="ClientCardStatusChangeRecoveryDto"/> dto</returns>
        Task<ClientCardStatusChangeRecoveryDto> CreateClientCardStatusChangeRecoveryDtoAsync(long clientCardId);


        /// <summary>
        /// Returns <see cref="ClientCard.StatusChanges"/> of <see cref="ClientCard"/>
        /// with <paramref name="clientCardId"/>
        /// </summary>
        /// <param name="clientCardId"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="ClientCardStatusChangeListDto"/></returns>
        Task<IEnumerable<ClientCardStatusChangeListDto>> GetClientCardStatusChangesAsync(long clientCardId);
    }
}
