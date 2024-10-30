using AutoMapper;
using Library.Core.Contracts.DbServices;
using Library.Core.Dto.ClientCardTypes;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.Entities;

namespace Library.Core.Services.DbServices
{
    public class ClientCardTypeService : IClientCardTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public ClientCardTypeService(IUnitOfWork repository, IMapper mapper)
        {
            unitOfWork = repository;
            this.mapper = mapper;
        }

        public async Task<bool> ClientCardTypeExistsAsync(long id)
        {
            ClientCardType? clientCardType = await unitOfWork.ClientCardTypeRepository.GetByIdAsync(id);
            return clientCardType != null;
        }

        public async Task<IEnumerable<ClientCardTypeListDto>> GetAllClientCardTypesAsync()
        {
            List<ClientCardTypeListDto> clientCardTypes = mapper.Map<List<ClientCardTypeListDto>>(await unitOfWork.ClientCardTypeRepository
                .AllReadOnlyAsync());



            return clientCardTypes;
        }
    }
}
