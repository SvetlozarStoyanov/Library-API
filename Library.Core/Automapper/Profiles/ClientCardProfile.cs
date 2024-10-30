using AutoMapper;
using Library.Core.Dto.ClientCards;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class ClientCardProfile : Profile
    {
        public ClientCardProfile()
        {
            CreateMap<ClientCard, ClientCardListDto>()
                .ForMember(cc => cc.Status, opt => opt.MapFrom(src => src.Status.ToString()));


            CreateMap<ClientCard, ClientCardCreateDto>();
            CreateMap<ClientCard, ClientCardReactivateDto>();
            CreateMap<ClientCard, ClientCardNestedListDto>();
        }
    }
}
