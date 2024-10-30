using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface IGenreBookService
    {
        /// <summary>
        /// Returns true if <see cref="GenreBook"/> with <paramref name="genreId"/>
        /// and <paramref name="bookId"/> exists
        /// </summary>
        /// <param name="genreId"></param>
        /// <param name="bookId"></param>
        /// <returns><see cref="bool"/> exists</returns>
        Task<bool> GenreBookExistsAsync(long genreId, long bookId);

        /// <summary>
        /// Creates a new <see cref="GenreBook"/> entity with <paramref name="genreId"/> and <paramref name="bookId"/>
        /// If <see cref="GenreBook"/> with same parameters already exists it sets it's <see cref="GenreBook.IsDeleted"/>
        /// to false
        /// </summary>
        /// <param name="genreId"></param>
        /// <param name="bookId"></param>
        /// <returns><see cref="int"/> genreBookId</returns>
        Task<long> LinkGenreAndBookAsync(long genreId, long bookId);

        /// <summary>
        /// Deletes <see cref="GenreBook"/> with <paramref name="genreId"/> and <paramref name="bookId"/>
        /// </summary>
        /// <param name="genreId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        Task DeleteGenreBookAsync(long genreId, long bookId);
    }
}
