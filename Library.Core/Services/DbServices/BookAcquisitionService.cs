using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.BookAcquisitions;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class BookAcquisitionService : IBookAcquisitionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BookAcquisitionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BookAcquisitionListDto>> GetBookAcquisitionsAsync(long bookId)
        {
            List<BookAcquisitionListDto> bookAcquisitions = mapper.Map<List<BookAcquisitionListDto>>(
                await unitOfWork.BookAcquisitionRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(ba => ba.BookId == bookId)
                .ToListAsync());

            if (bookAcquisitions.Any())
            {
                throw new NotFoundException($"Book with id {bookId} was not found!");
            }

            return bookAcquisitions;
        }


        public BookAcquisitionRestockDto CreateBookAcquisitionRestockDto(long bookId)
        {
            BookAcquisitionRestockDto dto = new BookAcquisitionRestockDto()
            {
                BookId = bookId,
                Time = DateTime.Now,
            };

            return dto;
        }

        public async Task<long> RestockBookAsync(long bookId, BookAcquisitionRestockDto dto)
        {
            Book? book = await unitOfWork.BookRepository.AllAsQueryable()
                .Where(b => b.Id == bookId)
                .Include(b => b.Acquisitions)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                throw new NotFoundException($"Book with id {bookId} was not found!");
            }

            BookAcquisition delivery = mapper.Map<BookAcquisition>(dto);
            delivery.Type = BookAcquisitionType.Restock;
            book.Acquisitions.Add(delivery);

            book.Quantity += delivery.Quantity;

            unitOfWork.BookRepository.Update(book);
            await unitOfWork.SaveChangesAsync();
            return delivery.Id;
        }
    }
}
