using Library.Core.Dto.Genres;
using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface IGenreService
    {
        /// <summary>
        /// Returns true if <see cref="Genre"/> with given <paramref name="id"/>
        /// exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> GenreExistsAsync(long id);

        /// <summary>
        /// Returns all <see cref="Genre"/> as <see cref="ICollection{T}"/> of <see cref="GenreListDto"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="GenreListDto"/></returns>
        Task<IEnumerable<GenreListDto>> GetAllGenresAsync();

        /// <summary>
        /// Returns all <see cref="Genre"/> as <see cref="ICollection{T}"/> of <see cref="GenreSelectDto"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="GenreSelectDto"/></returns>
        Task<IEnumerable<GenreSelectDto>> GetAllGenresForSelectAsync();

        /// <summary>
        /// Returns a <see cref="GenreDetailsDto"/> of <see cref="Genre"/>
        /// with given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="GenreDetailsDto"/> dto</returns>
        Task<GenreDetailsDto> GetGenreByIdAsync(long id);

        /// <summary>
        /// Creates a <see cref="GenreCreateDto"/> and returns it
        /// </summary>
        /// <returns><see cref="GenreCreateDto"/> dto</returns>
        GenreCreateDto CreateGenreCreateDtoAsync();

        /// <summary>
        /// Creates <see cref="Genre"/> from <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="int"/> genreId</returns>
        Task<long> CreateGenreAsync(GenreCreateDto dto);

        /// <summary>
        /// Returns <see cref="GenreEditDto"/> of <see cref="Genre"/>
        /// with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="GenreEditDto"/> dto</returns>
        Task<GenreEditDto> CreateGenreEditDtoAsync(long id);

        /// <summary>
        /// Updates <see cref="Genre"/> with given <paramref name="id"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateGenreAsync(long id, GenreEditDto dto);

        /// <summary>
        /// Deletes <see cref="Genre"/> with <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteGenreByIdAsync(long id);
    }
}
