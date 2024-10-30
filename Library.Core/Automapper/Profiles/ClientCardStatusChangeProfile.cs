using AutoMapper;
using Library.Core.Dto.ClientCardStatusChanges;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class ClientCardStatusChangeProfile : Profile
    {
        public ClientCardStatusChangeProfile()
        {
            CreateMap<ClientCardStatusChange, ClientCardStatusChangeListDto>()
                .ForMember(ccsc => ccsc.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(ccsc => ccsc.Reason, opt => opt.MapFrom(src => src.Reason.ToString()));
        }
    }
}
