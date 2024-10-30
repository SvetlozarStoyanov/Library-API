using Library.Core.Dto.ClientCardTypes;
using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface IClientCardTypeService
    {
        /// <summary>
        /// Checks if <see cref="ClientCardType"/> with given <paramref name="id"/>
        /// exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> ClientCardTypeExistsAsync(long id);

        /// <summary>
        /// Returns all <see cref="ClientCardType"/> as <see cref="IEnumerable{T}"/> of <see cref="ClientCardTypeListDto"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="ClientCardTypeListDto"/></returns>
        Task<IEnumerable<ClientCardTypeListDto>> GetAllClientCardTypesAsync();
    }
}
