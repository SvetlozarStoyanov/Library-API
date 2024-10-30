using Library.Core.Dto.Authors;
using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface IAuthorService
    {
        /// <summary>
        /// Returns <see cref="true"/> if <see cref="Author"/> with given <paramref name="id"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> AuthorExistsAsync(long id);

        /// <summary>
        /// Returns all <see cref="Author"/> as <see cref="IEnumerable{T}"/> of <see cref="AuthorListDto"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> <see cref="AuthorListDto"/></returns>
        Task<IEnumerable<AuthorListDto>> GetAllAuthorsAsync();

        /// <summary>
        /// Returns all <see cref="Author"/> as <see cref="IEnumerable{T}"/> of <see cref="AuthorSelectDto"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> <see cref="AuthorSelectDto"/></returns>
        Task<IEnumerable<AuthorSelectDto>> GetAllAuthorsForSelectAsync();

        /// <summary>
        /// Returns <see cref="AuthorDetailsDto"/> of <see cref="Author"/>
        /// with given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AuthorDetailsDto"/></returns>
        Task<AuthorDetailsDto> GetAuthorByIdAsync(long id);

        /// <summary>
        /// Creates an <see cref="AuthorCreateDto"/> and returns it
        /// </summary>
        /// <returns><see cref="AuthorCreateDto"/> dto</returns>
        AuthorCreateDto CreateAuthorCreateDtoAsync();

        /// <summary>
        /// Creates an <see cref="Author"/> from <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="int"/> authorId</returns>
        Task<long> CreateAuthorAsync(AuthorCreateDto dto);

        /// <summary>
        /// Creates an <see cref="AuthorCreateDto"/> for <see cref="Author"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="AuthorEditDto"/> dto</returns>
        Task<AuthorEditDto> CreateAuthorEditDtoAsync(long id);

        /// <summary>
        /// Edits <see cref="Author"/> with given <paramref name="id"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateAuthorAsync(long id, AuthorEditDto dto);

        /// <summary>
        /// Deletes <see cref="Author"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAuthorByIdAsync(long id);
    }
}
