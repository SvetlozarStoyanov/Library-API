using Library.Core.Dto.BookAcquisitions;
using Library.Infrastructure.Entities;

namespace Library.Core.Contracts.DbServices
{
    public interface IBookAcquisitionService
    {
        /// <summary>
        /// Returns <see cref="Book.Acquisitions"/> as <see cref="IEnumerable{T}"/> of <see cref="BookAcquisitionListDto"/>
        /// for <see cref="Book"/> with <paramref name="bookId"/>
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns><see cref="IEnumerable{T}"/> of <see cref="BookAcquisitionListDto"/></returns>
        Task<IEnumerable<BookAcquisitionListDto>> GetBookAcquisitionsAsync(long bookId);

        /// <summary>
        /// Creates a <see cref="BookAcquisitionRestockDto"/> for <see cref="Book"/>
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        BookAcquisitionRestockDto CreateBookAcquisitionRestockDto(long bookId);

        /// <summary>
        /// Creates <see cref="BookAcquisition"/>, with data from <see cref="BookAcquisitionRestockDto"/>,
        /// and adds <see cref="BookAcquisitionRestockDto.Quantity"/> to <see cref="Book.Quantity"/> 
        /// for <see cref="Book"/> with <paramref name="bookId"/>
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="dto"></param>
        /// <returns><see cref="BookAcquisition.Id"/></returns>
        Task<long> RestockBookAsync(long bookId, BookAcquisitionRestockDto dto);
    }
}
