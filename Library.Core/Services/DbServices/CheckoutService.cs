using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Checkouts;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CheckoutService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> CheckoutExistsAsync(long id)
        {
            Checkout? checkout = await unitOfWork.CheckoutRepository.GetByIdAsync(id);

            return checkout != null;
        }

        public async Task<bool> CheckoutIsFinalizedAsync(long id)
        {
            bool checkoutIsFinalized = await unitOfWork.CheckoutRepository
                .AllAsQueryable()
                .AsNoTracking()
                .AnyAsync(ch => ch.Id == id
                && (ch.Status == CheckoutStatus.Returned || ch.Status == CheckoutStatus.Unreturned));

            return checkoutIsFinalized;
        }

        public async Task<IEnumerable<CheckoutListDto>> GetFilteredCheckoutsAsync(CheckoutsFilterDto dto)
        {
            IQueryable<Checkout> checkouts = unitOfWork.CheckoutRepository
                .AllAsQueryable()
                .AsNoTracking();

            if (dto.CheckoutTimeMin > dto.CheckoutTimeMax)
            {
                throw new ArgumentException("CheckoutTimeMin cannot be bigger than CheckoutTimeMax!");
            }

            if (dto.Status != null)
            {
                checkouts = checkouts.Where(ch => ch.Status == dto.Status);
            }

            if (dto.CheckoutTimeMin != null)
            {
                checkouts = checkouts.Where(ch => ch.CheckoutTime >= dto.CheckoutTimeMin.Value);
            }

            if (dto.CheckoutTimeMax != null)
            {
                checkouts = checkouts.Where(ch => ch.CheckoutTime <= dto.CheckoutTimeMax);
            }

            List<CheckoutListDto> resultCheckouts = mapper.Map<List<CheckoutListDto>>(await checkouts
                .Include(ch => ch.ClientCard)
                .ThenInclude(cc => cc.Client)
                .Include(ch => ch.Book)
                .Skip(dto.ItemsPerPage * (dto.Page - 1))
                .Take(dto.ItemsPerPage)
                .ToListAsync());

            return resultCheckouts;
        }

        public async Task<IEnumerable<CheckoutListDto>> GetBookCheckoutsAsync(long bookId,
            int page = 1,
            int itemsPerPage = 6)
        {
            List<CheckoutListDto> checkouts = mapper.Map<List<CheckoutListDto>>(await unitOfWork.CheckoutRepository
                    .AllAsQueryable()
                    .AsNoTracking()
                    .Where(ch => ch.BookId == bookId)
                    .Include(ch => ch.ClientCard)
                    .ThenInclude(cc => cc.Client)
                    .Skip(itemsPerPage * (page - 1))
                    .Take(itemsPerPage)
                    .ToListAsync());

            return checkouts;
        }

        public async Task<IEnumerable<CheckoutListDto>> GetClientCardCheckoutsAsync(long clientCardId,
            int page = 1,
            int itemsPerPage = 6)
        {
            List<CheckoutListDto> checkouts = mapper.Map<List<CheckoutListDto>>(await unitOfWork.CheckoutRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(ch => ch.ClientCardId == clientCardId)
                .Include(ch => ch.Book)
                .Skip(itemsPerPage * (page - 1))
                .Take(itemsPerPage)
                .ToListAsync());

            return checkouts;
        }

        public async Task<CheckoutDetailsDto> GetCheckoutByIdAsync(long id)
        {
            CheckoutDetailsDto? checkout = mapper.Map<CheckoutDetailsDto?>(await unitOfWork.CheckoutRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(ch => ch.Id == id)
                .Include(ch => ch.ClientCard)
                .ThenInclude(cc => cc.Client)
                .Include(ch => ch.Book)
                .FirstOrDefaultAsync());

            if (checkout == null)
            {
                throw new NotFoundException($"Checkout with id {id} was not found!");
            }

            return checkout;
        }

        public async Task<CheckoutCreateDto> CreateCheckoutCreateDtoAsync(long clientCardId)
        {
            ClientCard? clientCardDto = await unitOfWork.ClientCardRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(cc => cc.Id == clientCardId)

                .FirstOrDefaultAsync();

            if (clientCardDto == null)
            {
                throw new NotFoundException($"Client card with id {clientCardId} was not found!");
            }

            CheckoutCreateDto dto = new CheckoutCreateDto()
            {
                CheckoutTime = DateTime.Now,
                ClientCardId = clientCardId
            };

            return dto;
        }

        public async Task<long> CreateCheckoutAsync(long clientCardId, CheckoutCreateDto dto)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(cc => cc.Id == clientCardId)
                .Include(cc => cc.ClientCardType)
                .Include(cc => cc.Client)
                .FirstOrDefaultAsync();

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {clientCardId} was not found!");
            }

            Checkout checkout = new Checkout();
            checkout.ClientCardId = clientCardId;
            checkout.BookId = dto.BookId;
            checkout.CheckoutTime = dto.CheckoutTime;
            checkout.DueTime = dto.CheckoutTime.AddDays(clientCard.ClientCardType.CheckoutTimeLimit);

            Book? book = await unitOfWork.BookRepository.GetByIdAsync(checkout.BookId);

            if (book == null)
            {
                throw new NotFoundException($"Book with id {checkout.BookId} was not found!");
            }

            if (book.Quantity < 1)
            {
                throw new InvalidOperationException("Cannot checkout such quantity!");
            }
            book.Quantity -= 1;

            await unitOfWork.CheckoutRepository.AddAsync(checkout);
            unitOfWork.BookRepository.Update(book);
            await unitOfWork.SaveChangesAsync();
            return checkout.Id;
        }

        public async Task<CheckoutFinalizationDto> CreateCheckoutFinalizationDto(long id)
        {
            CheckoutFinalizationDto? dto = await unitOfWork.CheckoutRepository.AllAsQueryable()
                .Where(ch => ch.Id == id)
                .Select(ch => new CheckoutFinalizationDto()
                {
                    Id = ch.Id,
                    BookIsReturned = true,
                    Time = DateTime.Now
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                throw new NotFoundException($"Checkout with id {id} was not found!");
            }

            return dto;
        }

        public async Task FinalizeCheckoutAsync(long id, CheckoutFinalizationDto dto)
        {
            Checkout? checkout = await unitOfWork.CheckoutRepository.AllAsQueryable()
                .Where(ch => ch.Id == id)
                .Include(ch => ch.Book)
                .FirstOrDefaultAsync();

            if (checkout == null)
            {
                throw new NotFoundException($"Checkout with id {id} was not found!");
            }

            if (checkout.Status == CheckoutStatus.Returned || checkout.Status == CheckoutStatus.Unreturned)
            {
                throw new InvalidOperationException("Checkout is already finalized!");
            }

            if (dto.BookIsReturned)
            {
                checkout.Status = CheckoutStatus.Returned;
                checkout.Book.Quantity += 1;
                checkout.ReturnTime = dto.Time;
            }
            else
            {
                checkout.Status = CheckoutStatus.Unreturned;
            }

            unitOfWork.CheckoutRepository.Update(checkout);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCheckoutAsync(long id)
        {
            Checkout? checkout = await unitOfWork.CheckoutRepository.GetByIdAsync(id);

            if (checkout == null)
            {
                throw new NotFoundException($"Checkout with id {id} was not found!");
            }

            await unitOfWork.CheckoutRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
