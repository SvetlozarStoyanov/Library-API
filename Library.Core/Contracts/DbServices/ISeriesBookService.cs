using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface ISeriesBookService
    {
        /// <summary>
        /// Returns true if <see cref="SeriesBook"/> with <paramref name="seriesId"/>
        /// and <paramref name="bookId"/> exists
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="bookId"></param>
        /// <returns><see cref="bool"/></returns>
        Task<bool> SeriesBookExistsAsync(long seriesId, long bookId);

        /// <summary>
        /// Creates a new <see cref="SeriesBook"/> entity with <paramref name="seriesId"/> and <paramref name="bookId"/>
        /// If <see cref="SeriesBook"/> with same parameters already exists it sets it's <see cref="SeriesBook.IsDeleted"/>
        /// to false
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="bookId"></param>
        /// <returns><see cref="int"/> seriesBookId</returns>
        Task<long> LinkSeriesAndBookAsync(long seriesId, long bookId);

        /// <summary>
        /// Deletes <see cref="SeriesBook"/> with <paramref name="seriesId"/> and <paramref name="bookId"/>,
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        Task DeleteSeriesBookAsync(long seriesId, long bookId);
    }
}
