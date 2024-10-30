using AutoMapper;
using Library.Core.Dto.ClientCardTypes;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class ClientCardTypeProfile : Profile
    {
        public ClientCardTypeProfile()
        {
            CreateMap<ClientCardType, ClientCardTypeListDto>();
        }
    }
}
