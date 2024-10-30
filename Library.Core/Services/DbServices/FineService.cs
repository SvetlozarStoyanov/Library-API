using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.Fines;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Library.Core.Services.DbServices
{
    public class FineService : IFineService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public FineService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> FineExistsAsync(long id)
        {
            Fine? fine = await unitOfWork.FineRepository.GetByIdAsync(id);

            return fine != null;
        }

        public async Task<bool> FineIsUnpaidAsync(long id)
        {
            Fine? fine = await unitOfWork.FineRepository.GetByIdAsync(id);

            if (fine == null)
            {
                throw new NotFoundException($"Fine with {id} was not found!");
            }

            return fine.Status == FineStatus.Unpaid;
        }

        public async Task<bool> CheckoutHasUnpaidFineAsync(long checkoutId)
        {
            List<Fine> fines = await unitOfWork.FineRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(f => f.CheckoutId == checkoutId)
                .ToListAsync();

            return fines != null && fines.Any(f => f.Status == FineStatus.Unpaid);
        }

        public async Task<bool> CheckoutHasSameFineAsync(long checkoutId, FineReason fineReason)
        {
            bool fineWithSameTypeExists = await unitOfWork.FineRepository
                .AllAsQueryable()
                .AsNoTracking()
                .AnyAsync(f => f.CheckoutId == checkoutId && f.Reason == fineReason);

            return fineWithSameTypeExists;
        }

        public async Task<IEnumerable<FineListDto>> GetClientFinesAsync(long clientId)
        {
            List<FineListDto> fines = mapper.Map<List<FineListDto>>(await unitOfWork.FineRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(f => f.Checkout.ClientCard.ClientId == clientId && f.Status != FineStatus.Outdated)
                .ToListAsync());

            return fines;
        }

        public async Task<IEnumerable<FineListDto>> GetFineHistoryByCodeAsync(string code)
        {
            List<FineListDto> fines = mapper.Map<List<FineListDto>>(await unitOfWork.FineRepository
                .AllReadOnlyAsync(f => f.Code == code));

            return fines;
        }

        public async Task<FineCreateDto> CreateFineCreateDtoAsync(long checkoutId)
        {
            Checkout? checkout = await unitOfWork.CheckoutRepository.GetByIdAsync(checkoutId);

            if (checkout == null)
            {
                throw new NotFoundException($"Checkout was with id {checkoutId} not found!");
            }

            FineCreateDto fineDto = new FineCreateDto();
            fineDto.Reason = FineReason.LostItem;
            fineDto.CheckoutId = checkoutId;
            fineDto.Date = DateTime.Now;

            return fineDto;
        }

        public async Task<long> CreateFineAsync(FineCreateDto dto)
        {
            Checkout? checkout = await unitOfWork.CheckoutRepository.AllAsQueryable()
                .Where(ch => ch.Id == dto.CheckoutId)
                .Include(ch => ch.ClientCard)
                .Include(ch => ch.Fines)
                .FirstOrDefaultAsync();

            if (checkout == null)
            {
                throw new NotFoundException($"Checkout with id {dto.CheckoutId} was not found!");
            }

            if (checkout.Fines.Where(f => f.Status == FineStatus.Unpaid).Any(f => f.Reason == dto.Reason))
            {
                throw new InvalidOperationException($"Cannot create {dto.Reason} fine because checkout with id {checkout.Id} already has an unpaid fine of same kind!");
            }

            if (dto.Date < checkout.DueTime && dto.Reason == FineReason.ReturnDelay)
            {
                throw new InvalidOperationException("Cannot issue a late return fine for a checkout which has a due time that has not yet passed!");
            }

            Fine fine = mapper.Map<Fine>(dto);
            fine.Code = await GenerateFineCodeAsync();
            fine.Status = FineStatus.Unpaid;

            await unitOfWork.FineRepository.AddAsync(fine);
            await unitOfWork.SaveChangesAsync();

            return fine.Id;
        }

        public async Task<FinePaymentDto> CreateFinePaymentDtoAsync(long id)
        {
            FinePaymentDto? fine = await unitOfWork.FineRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(f => f.Id == id && f.Status == FineStatus.Unpaid)
                .Select(f => new FinePaymentDto()
                {
                    Id = f.Id,
                    Date = DateTime.Now,
                })
                .FirstOrDefaultAsync();

            if (fine == null)
            {
                throw new NotFoundException($"Fine with {id} was not found!");
            }

            return fine;
        }

        public async Task PayFineAsync(long id, FinePaymentDto dto)
        {
            Fine? fine = await unitOfWork.FineRepository.AllAsQueryable()
                .Where(f => f.Id == id)
                .Include(f => f.Checkout)
                .FirstOrDefaultAsync();

            if (fine == null)
            {
                throw new NotFoundException($"Fine with {id} was not found!");
            }

            if (fine.Status == FineStatus.Waived)
            {
                throw new InvalidOperationException("Cannot pay for a waived fine!");
            }
            else if (fine.Status == FineStatus.Paid)
            {
                throw new InvalidOperationException("Cannot pay for a already paid fine!");
            }

            if (fine.IssueDate > dto.Date)
            {
                throw new ArgumentException("Payment date cannot be before issue date!");
            }

            fine.Status = FineStatus.Paid;

            unitOfWork.FineRepository.Update(fine);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<FineAdjustmentDto> CreateFineAdjustmentDtoAsync(long id)
        {
            FineAdjustmentDto? dto = await unitOfWork.FineRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(f => f.Id == id)
                .Select(f => new FineAdjustmentDto()
                {
                    Id = f.Id,
                    Date = DateTime.Now,
                    OldAmount = f.Amount,
                    NewAmount = 0
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                throw new NotFoundException($"Fine with {id} was not found!");
            }

            return dto;
        }

        public async Task AdjustFineAsync(long id, FineAdjustmentDto dto)
        {
            Fine? fine = await unitOfWork.FineRepository.GetByIdAsync(id);

            if (fine == null)
            {
                throw new NotFoundException($"Fine with {id} was not found!");
            }

            if (fine.Status != FineStatus.Unpaid)
            {
                throw new InvalidOperationException($"Fine with {id} cannot be updated because it is {fine.Status}!");
            }

            if (fine.IssueDate > dto.Date)
            {
                throw new ArgumentException("Change date cannot be before issue date!");
            }

            if (fine.Amount <= 0)
            {
                throw new InvalidOperationException("Fine amount must be bigger than 0!");
            }

            fine.Status = FineStatus.Outdated;

            Fine? newFine = new Fine();

            newFine.Status = FineStatus.Unpaid;
            newFine.Reason = fine.Reason;
            newFine.Code = fine.Code;
            newFine.CheckoutId = fine.CheckoutId;
            newFine.Amount = dto.NewAmount;
            newFine.IssueDate = dto.Date;

            unitOfWork.FineRepository.Update(fine);
            await unitOfWork.FineRepository.AddAsync(newFine);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<FineWaiverDto> CreateFineWaiveDtoAsync(long id)
        {
            FineWaiverDto? dto = await unitOfWork.FineRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(f => f.Id == id && f.Status == FineStatus.Unpaid)
                .Select(f => new FineWaiverDto()
                {
                    Id = f.Id,
                    Date = DateTime.Now
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                throw new NotFoundException($"Fine with {id} was not found!");
            }

            return dto;
        }

        public async Task WaiveFineAsync(long id, FineWaiverDto dto)
        {
            Fine? fine = await unitOfWork.FineRepository.GetByIdAsync(id);

            if (fine == null)
            {
                throw new NotFoundException($"Fine with {id} was not found!");
            }

            if (fine.Status != FineStatus.Unpaid)
            {
                throw new InvalidOperationException($"Fine cannot be waived because it is {fine.Status}!");
            }

            if (fine.IssueDate > dto.Date)
            {
                throw new ArgumentException("Waiver date cannot be before issue date!");
            }

            fine.Status = FineStatus.Waived;

            unitOfWork.FineRepository.Update(fine);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task<string> GenerateFineCodeAsync()
        {
            List<string> fineCodes = await unitOfWork.FineRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Select(f => f.Code)
                .ToListAsync();

            StringBuilder sb = new StringBuilder();

            Random random = new Random();
            char letterOne = (char)random.Next(65, 91);
            char letterTwo = (char)random.Next(65, 91);

            sb.Append($"{letterOne}{letterTwo}-{random.Next(10000000, 100000000)}");

            while (fineCodes.Contains(sb.ToString()))
            {
                sb.Clear();
                sb.Append($"{letterOne}{letterTwo}-{random.Next(10000000, 100000000)}");
            }

            return sb.ToString();
        }
    }
}
