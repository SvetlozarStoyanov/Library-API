using AutoMapper;
using Library.Core.Common.CustomExceptions;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.ClientCardStatusChanges;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Core.Services.DbServices
{
    public class ClientCardStatusChangeService : IClientCardStatusChangeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ClientCardStatusChangeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ClientCardStatusChangeRecoveryDto> CreateClientCardStatusChangeRecoveryDtoAsync(long clientCardId)
        {
            ClientCard? clientCard = await unitOfWork.ClientCardRepository.GetByIdAsync(clientCardId);

            if (clientCard == null)
            {
                throw new NotFoundException($"Client card with id {clientCardId} was not found!");
            }

            if (clientCard.Status != ClientCardStatus.Lost)
            {
                throw new InvalidOperationException("Cannot recover a client card which is not lost!");
            }

            ClientCardStatusChangeRecoveryDto dto = new ClientCardStatusChangeRecoveryDto();
            dto.ClientCardId = clientCardId;
            dto.UpdatedOn = DateTime.UtcNow;
            dto.ReactivateClientCard = true;

            return dto;
        }

        public async Task<IEnumerable<ClientCardStatusChangeListDto>> GetClientCardStatusChangesAsync(long clientCardId)
        {
            List<ClientCardStatusChangeListDto> clientCardStatusChanges = mapper.Map<List<ClientCardStatusChangeListDto>>
                                                                                (await unitOfWork.ClientCardStatusChangeRepository
                                                                                .AllAsQueryable()
                                                                                .AsNoTracking()
                                                                                .Where(ccsc => ccsc.ClientCardId == clientCardId)
                                                                                .ToListAsync());

            return clientCardStatusChanges;
        }
    }
}
