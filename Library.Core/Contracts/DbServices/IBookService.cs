using Library.Core.Dto.Books;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;

namespace Library.Core.Contracts.DbServices
{
    public interface IBookService
    {
        /// <summary>
        /// Returns true if <see cref="Book"/> with given <paramref name="id"/>
        /// exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> BookExistsAsync(long id);

        /// <summary>
        /// Checks if <see cref="Book"/> with <paramref name="bookId"/>
        /// has <see cref="Book.Quantity"/> more than <paramref name="quantity"/>
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="quantity"></param>
        /// <returns><see cref="bool"/> isSufficient</returns>
        Task<bool> BookQuantityIsSufficientAsync(long bookId, int quantity);

        /// <summary>
        /// Returns all <see cref="Book"/>> which match the given criteria
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="BookListDto"/></returns>
        Task<IEnumerable<BookListDto>> GetFilteredBooksAsync(BooksFilterDto dto);

        /// <summary>
        /// Returns all <see cref="Book"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="BookListDto"/></returns>
        Task<IEnumerable<BookListDto>> GetAllBooksAsync();

        /// <summary>
        /// Returns <see cref="Book"/> as a <see cref="BookDetailsDto"/> with given
        /// <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="BookDetailsDto"/></returns>
        Task<BookDetailsDto> GetBookByIdAsync(long id);

        /// <summary>
        /// Creates and returns a <see cref="BookCreateDto"/>
        /// </summary>
        /// <returns><see cref="BookCreateDto"/> dto</returns>
        BookCreateDto CreateBookCreateDtoAsync();

        /// <summary>
        /// Creates <see cref="Book"/> from <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="int"/> id</returns>
        Task<long> CreateBookAsync(BookCreateDto dto);

        /// <summary>
        /// Creates an <see cref="BookExperimentalCreateDto"/> dto
        /// </summary>
        /// <returns><see cref="BookExperimentalCreateDto"/> dto</returns>
        BookExperimentalCreateDto CreateBookExperimentalCreateDto();

        /// <summary>
        /// Creates a <see cref="Book"/> and fills its <see cref="Book.Authors"/>, <see cref="Book.Genres"/>
        /// and <see cref="Book.Series"/> with data from <see cref="BookExperimentalCreateDto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns><see cref="Book.Id"/></returns>
        Task<long> ExperimentalCreateBookAsync(BookExperimentalCreateDto dto);

        /// <summary>
        /// Returns <see cref="BookEditDto"/> of <see cref="Book"/>
        /// with given <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="BookEditDto"/> dto</returns>
        Task<BookEditDto> CreateBookEditDtoAsync(long id);

        /// <summary>
        /// Updates a <see cref="Book"/> with given <paramref name="id"/>
        /// with data from <paramref name="dto"/>
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateBookAsync(long id, BookEditDto dto);

        /// <summary>
        /// Sets the <see cref="Book.Status"/> property of <see cref="Book"/>
        /// with given <paramref name="id"/> to <see cref="BookStatus.Archived"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ArchiveBookByIdAsync(long id);
    }
}
