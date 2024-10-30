using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class GenreBookService : IGenreBookService
    {
        private readonly IUnitOfWork unitOfWork;

        public GenreBookService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> GenreBookExistsAsync(long genreId, long bookId)
        {
            bool genreBookExists = await unitOfWork.GenreBookRepository
                .AllAsQueryable()
                .AsNoTracking()
                .AnyAsync(gb => gb.GenreId == genreId
                && gb.BookId == bookId);

            return genreBookExists;
        }

        public async Task<long> LinkGenreAndBookAsync(long genreId, long bookId)
        {
            GenreBook? genreBook = await unitOfWork.GenreBookRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(gb => gb.GenreId == genreId && gb.BookId == bookId)
                .FirstOrDefaultAsync();

            if (genreBook != null)
            {
                throw new InvalidOperationException("Genre and book are already linked!");
            }

            GenreBook newGenreBook = new GenreBook()
            {
                GenreId = genreId,
                BookId = bookId
            };

            await unitOfWork.GenreBookRepository.AddAsync(newGenreBook);
            await unitOfWork.SaveChangesAsync();
            return newGenreBook.Id;
        }

        public async Task DeleteGenreBookAsync(long genreId, long bookId)
        {
            GenreBook? genreBook = await unitOfWork.GenreBookRepository.AllAsQueryable()
                .FirstOrDefaultAsync(gb => gb.GenreId == genreId
                && gb.BookId == bookId);

            if (genreBook == null)
            {
                throw new NotFoundException("Book does not belong to genre!");
            }

            await unitOfWork.GenreBookRepository.DeleteAsync(genreBook.Id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
