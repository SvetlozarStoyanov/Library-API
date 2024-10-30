using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface IAuthorBookService
    {
        /// <summary>
        /// Returns true if <see cref="AuthorBook"/> with <paramref name="authorId"/>
        /// and <paramref name="bookId"/> exists
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="bookId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> AuthorBookExistsAsync(long authorId, long bookId);

        /// <summary>
        /// Creates a new <see cref="AuthorBook"/> entity with <paramref name="authorId"/> and <paramref name="bookId"/>
        /// If <see cref="AuthorBook"/> with same already parameters exists it sets it's <see cref="AuthorBook.IsDeleted"/>
        /// to false
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="bookId"></param>
        /// <returns><see cref="int"/> authorBookId</returns>
        Task<long> LinkAuthorAndBookAsync(long authorId, long bookId);

        /// <summary>
        /// Deletes <see cref="AuthorBook"/> with <paramref name="authorId"/> and <paramref name="bookId"/>,
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        Task DeleteAuthorBookAsync(long authorId, long bookId);
    }
}
