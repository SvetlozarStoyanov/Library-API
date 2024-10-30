using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.ClientCards;
using Library.Core.Dto.ClientCardStatusChanges;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class ClientCardService : IClientCardService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ClientCardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<bool> ClientCardExistsAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(id);

            return clientCard != null;
        }

        public async Task<bool> CanReactivateClientCardAsync(long id)
        {
            bool canBeReactivated = await unitOfWork.ClientCardRepository.AllAsQueryable().AsNoTracking()
                .AnyAsync(cc => cc.Id == id && cc.Status == ClientCardStatus.Inactive);

            return canBeReactivated;
        }

        public async Task<bool> ClientHasSameTypeOfCardAsync(long clientId, long clientCardTypeId)
        {
            bool clientHasSameTypeOfCardAsync = await unitOfWork.ClientCardRepository.AllAsQueryable().AsNoTracking()
                .AnyAsync(cc => cc.ClientId == clientId
                && cc.ClientCardTypeId == clientCardTypeId
                && (cc.Status == ClientCardStatus.Active
                || cc.Status == ClientCardStatus.Inactive
                || cc.Status == ClientCardStatus.Suspended));

            return clientHasSameTypeOfCardAsync;
        }

        public async Task<bool> ClientCardHasUnfinalizedCheckoutsAsync(long id)
        {
            bool clientCardHasUnfinalizedCheckouts = await unitOfWork.ClientCardRepository.AllAsQueryable().AsNoTracking()
                .Where(cc => cc.Id == id)
                .Select(cc => cc.Checkouts)
                .AnyAsync(ch => ch.Any(ch => ch.Status == CheckoutStatus.Pending));

            return clientCardHasUnfinalizedCheckouts;
        }

        public async Task<bool> ClientCardIsExpiredAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(id);

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            return clientCard.Status == ClientCardStatus.Expired;
        }

        public async Task<bool> ClientCardCanCreateCheckoutsAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.AllAsQueryable()
                .AsNoTracking()
                .Where(cc => cc.Id == id)
                .Include(cc => cc.Checkouts)
                .Include(cc => cc.ClientCardType)
                .FirstOrDefaultAsync();

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            return clientCard.Status == ClientCardStatus.Active
                && clientCard.ClientCardType.CheckoutQuantityLimit - clientCard.Checkouts.Count(ch => ch.Status == CheckoutStatus.Pending) > 0;
        }

        public async Task<int> GetClientCardAvailableCheckoutQuantityAsync(long id)
        {
            int creditCardQuantityLimit = await unitOfWork.ClientCardRepository.AllAsQueryable().AsNoTracking()
                .Where(cc => cc.Id == id)
                .Select(cc => cc.ClientCardType.CheckoutQuantityLimit - cc.Checkouts.Count(ch => ch.Status == CheckoutStatus.Pending))
                .FirstOrDefaultAsync();

            return creditCardQuantityLimit;
        }

        public async Task<long> GetClientCardClientIdAsync(long id)
        {
            long clientId = await unitOfWork.ClientCardRepository.AllAsQueryable()
                .AsNoTracking()
                .Where(cc => cc.Id == id)
                .Select(cc => cc.ClientId)
                .FirstOrDefaultAsync();

            if (clientId == 0)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            return clientId;
        }

        public async Task<IEnumerable<ClientCardListDto>> GetClientCardsByClientIdAsync(long clientId)
        {
            List<ClientCardListDto> clientCards = mapper.Map<List<ClientCardListDto>>(await unitOfWork.ClientCardRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(cc => cc.ClientId == clientId)
                .Include(cc => cc.ClientCardType)
                .OrderBy(cc => cc.Status)
                .ToListAsync());

            return clientCards;
        }

        public async Task<int> GetClientCardCheckoutTimeLimitAsync(long id)
        {
            int clientCardCheckoutTimeLimit = await unitOfWork.ClientCardRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(cc => cc.Id == id)
                .Select(cc => cc.ClientCardType.CheckoutTimeLimit)
                .FirstOrDefaultAsync();

            if (clientCardCheckoutTimeLimit == 0)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            return clientCardCheckoutTimeLimit;
        }

        public ClientCardCreateDto CreateClientCardCreateDto(long clientId)
        {
            ClientCardCreateDto dto = new ClientCardCreateDto()
            {
                ClientId = clientId,
                CreationDate = DateTime.Now
            };

            return dto;
        }

        public async Task<long> CreateCreditClientCardAsync(long clientId, ClientCardCreateDto dto)
        {
            ClientCardType? clientCardType = await unitOfWork.ClientCardTypeRepository.GetByIdAsync(dto.ClientCardTypeId);

            if (clientCardType == null)
            {
                throw new NotFoundException($"Client card type with id {dto.ClientCardTypeId} type was not found!");
            }

            ClientCard clientCard = new ClientCard();
            clientCard.ClientId = clientId;
            clientCard.ClientCardTypeId = dto.ClientCardTypeId;
            clientCard.CreationDate = dto.CreationDate;
            clientCard.ExpirationDate = clientCard.CreationDate.AddMonths(clientCardType.MonthsValid);
            clientCard.Status = ClientCardStatus.Active;

            ClientCardStatusChange statusChange = new ClientCardStatusChange();
            statusChange.Reason = ClientCardStatusChangeReason.InitialActivation;
            statusChange.Status = clientCard.Status;
            statusChange.UpdatedOn = DateTime.Now;
            clientCard.StatusChanges.Add(statusChange);

            await unitOfWork.ClientCardRepository.AddAsync(clientCard);
            await unitOfWork.SaveChangesAsync();
            return clientCard.Id;
        }

        public async Task<ClientCardReactivateDto> CreateClientCardReactivateDtoAsync(long id)
        {
            ClientCardReactivateDto? dto = mapper.Map<ClientCardReactivateDto>(await unitOfWork.ClientCardRepository
                .AllAsQueryable()
                .AsNoTracking()
                .Where(cc => cc.Id == id)
                .FirstOrDefaultAsync());

            if (dto == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            return dto;
        }

        public async Task ReactivateClientCardAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(id);

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            if (clientCard.Status != ClientCardStatus.Inactive)
            {
                throw new InvalidOperationException($"Client card cannot be reactivated because it is {clientCard.Status}!");
            }

            clientCard.Status = ClientCardStatus.Active;

            ClientCardStatusChange statusChange = new ClientCardStatusChange();
            statusChange.Reason = ClientCardStatusChangeReason.ClientReactivation;
            statusChange.Status = ClientCardStatus.Active;
            statusChange.UpdatedOn = DateTime.Now;
            clientCard.StatusChanges.Add(statusChange);

            unitOfWork.ClientCardRepository.Update(clientCard);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeactivateClientCardAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(id);

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            if (clientCard.Status != ClientCardStatus.Active)
            {
                throw new InvalidOperationException($"Client card cannot be deactivated because it is {clientCard.Status}");
            }

            clientCard.Status = ClientCardStatus.Inactive;

            ClientCardStatusChange statusChange = new ClientCardStatusChange();
            statusChange.Reason = ClientCardStatusChangeReason.ClientDeactivation;
            statusChange.Status = ClientCardStatus.Inactive;
            statusChange.UpdatedOn = DateTime.Now;
            clientCard.StatusChanges.Add(statusChange);

            unitOfWork.ClientCardRepository.Update(clientCard);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RenewClientCardAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository
                .AllAsQueryable()
                .Where(cc => cc.Id == id)
                .Include(cc => cc.ClientCardType)
                .FirstOrDefaultAsync();

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            if (!(clientCard.Status == ClientCardStatus.Active || clientCard.Status == ClientCardStatus.Expired))
            {
                throw new InvalidOperationException($"Client card cannot be renewed because it is {clientCard.Status}");
            }

            clientCard.Status = ClientCardStatus.Active;
            clientCard.ExpirationDate.AddMonths(clientCard.ClientCardType.MonthsValid);

            ClientCardStatusChange statusChange = new ClientCardStatusChange();
            statusChange.Reason = ClientCardStatusChangeReason.Renewal;
            statusChange.Status = ClientCardStatus.Active;
            statusChange.UpdatedOn = DateTime.Now;

            clientCard.StatusChanges.Add(statusChange);

            unitOfWork.ClientCardRepository.Update(clientCard);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task LoseClientCardAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(id);

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with {id} was not found!");
            }

            //if (clientCard.Status != ClientCardStatus.Active && clientCard.Status != ClientCardStatus.Inactive)
            //{
            //    throw new InvalidOperationException("Invalid operation!");
            //}
            clientCard.Status = ClientCardStatus.Lost;

            ClientCardStatusChange statusChange = new ClientCardStatusChange();
            statusChange.Reason = ClientCardStatusChangeReason.Loss;
            statusChange.Status = ClientCardStatus.Lost;
            statusChange.UpdatedOn = DateTime.Now;
            clientCard.StatusChanges.Add(statusChange);

            unitOfWork.ClientCardRepository.Update(clientCard);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task RecoverClientCardAsync(long id, ClientCardStatusChangeRecoveryDto dto)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(id);

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            if (clientCard.Status != ClientCardStatus.Lost)
            {
                throw new InvalidOperationException($"Cannot recover a card because is not lost!");
            }

            ClientCardStatusChange statusChange = new ClientCardStatusChange();
            statusChange.Reason = ClientCardStatusChangeReason.Recovery;

            statusChange.Status = dto.ReactivateClientCard ? ClientCardStatus.Active : ClientCardStatus.Inactive;

            clientCard.Status = statusChange.Status;

            statusChange.UpdatedOn = DateTime.Now;
            clientCard.StatusChanges.Add(statusChange);

            unitOfWork.ClientCardRepository.Update(clientCard);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task SuspendClientCardAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(id);

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            //if (clientCard.Status != ClientCardStatus.Active)
            //{
            //    throw new InvalidOperationException("Cannot suspend a card which is not active!");
            //}
            clientCard.Status = ClientCardStatus.Suspended;

            ClientCardStatusChange statusChange = new ClientCardStatusChange();
            statusChange.Reason = ClientCardStatusChangeReason.Suspension;
            statusChange.Status = ClientCardStatus.Suspended;
            statusChange.UpdatedOn = DateTime.Now;
            clientCard.StatusChanges.Add(statusChange);

            unitOfWork.ClientCardRepository.Update(clientCard);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task UnsuspendClientCardAsync(long id)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(id);

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {id} was not found!");
            }

            if (clientCard.Status != ClientCardStatus.Suspended)
            {
                throw new InvalidOperationException($"Cannot unsuspend a card because it is {clientCard.Status}!");
            }
            clientCard.Status = ClientCardStatus.Active;

            ClientCardStatusChange statusChange = new ClientCardStatusChange();
            statusChange.Reason = ClientCardStatusChangeReason.Unsuspension;
            statusChange.Status = ClientCardStatus.Active;
            statusChange.UpdatedOn = DateTime.Now;
            clientCard.StatusChanges.Add(statusChange);
            unitOfWork.ClientCardRepository.Update(clientCard);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
