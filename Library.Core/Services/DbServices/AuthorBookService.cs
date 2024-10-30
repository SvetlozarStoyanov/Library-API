using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class AuthorBookService : IAuthorBookService
    {
        private readonly IUnitOfWork unitOfWork;

        public AuthorBookService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> AuthorBookExistsAsync(long authorId, long bookId)
        {
            bool authorBookExists = await unitOfWork.AuthorBookRepository.AllAsQueryable()
                .AnyAsync(ab => ab.AuthorId == authorId
                && ab.BookId == bookId);

            return authorBookExists;
        }

        public async Task<long> LinkAuthorAndBookAsync(long authorId, long bookId)
        {
            AuthorBook? authorBook = await unitOfWork.AuthorBookRepository
                .AllAsQueryable()
                .Where(gb => gb.AuthorId == authorId && gb.BookId == bookId)
                .FirstOrDefaultAsync();

            if (authorBook != null)
            {
                throw new InvalidOperationException("Author and book are already linked!");
            }

            AuthorBook newAuthorBook = new AuthorBook()
            {
                AuthorId = authorId,
                BookId = bookId,
            };

            await unitOfWork.AuthorBookRepository.AddAsync(newAuthorBook);
            await unitOfWork.SaveChangesAsync();
            return newAuthorBook.Id;
        }

        public async Task DeleteAuthorBookAsync(long authorId, long bookId)
        {
            AuthorBook? authorBook = await unitOfWork.AuthorBookRepository
                 .AllAsQueryable()
                 .FirstOrDefaultAsync(ab => ab.AuthorId == authorId
                 && ab.BookId == bookId);

            if (authorBook == null)
            {
                throw new NotFoundException("Book does not belong to author!");
            }

            await unitOfWork.AuthorBookRepository.DeleteAsync(authorBook.Id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
