using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class SeriesBookService : ISeriesBookService
    {
        private readonly IUnitOfWork unitOfWork;

        public SeriesBookService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> SeriesBookExistsAsync(long seriesId, long bookId)
        {
            bool seriesBookExists = await unitOfWork.SeriesBookRepository
                .AllAsQueryable()
                .AsNoTracking()
                .AnyAsync(sb => sb.SeriesId == seriesId
                && sb.BookId == bookId);

            return seriesBookExists;
        }

        public async Task<long> LinkSeriesAndBookAsync(long seriesId, long bookId)
        {
            SeriesBook? seriesBook = await unitOfWork.SeriesBookRepository.AllAsQueryable()
                .Where(gb => gb.SeriesId == seriesId && gb.BookId == bookId)
                .FirstOrDefaultAsync();

            if (seriesBook != null)
            {
                throw new InvalidOperationException("Series and book are already linked!");
            }
            SeriesBook newSeriesBook = new SeriesBook()
            {
                SeriesId = seriesId,
                BookId = bookId
            };
            await unitOfWork.SeriesBookRepository.AddAsync(newSeriesBook);
            await unitOfWork.SaveChangesAsync();
            return newSeriesBook.Id;
        }

        public async Task DeleteSeriesBookAsync(long seriesId, long bookId)
        {
            SeriesBook? seriesBook = await unitOfWork.SeriesBookRepository.AllAsQueryable()
                .FirstOrDefaultAsync(sb => sb.SeriesId == seriesId
                && sb.BookId == bookId);

            if (seriesBook == null)
            {
                throw new NotFoundException("Book does not belong to series");
            }

            await unitOfWork.SeriesBookRepository.DeleteAsync(seriesBook.Id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
