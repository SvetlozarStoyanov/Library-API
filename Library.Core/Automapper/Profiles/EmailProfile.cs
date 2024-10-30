using AutoMapper;
using Library.Core.Dto.Emails;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {
            CreateMap<Email, EmailListDto>()
                .ForMember(e => e.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<Email, EmailCreateOrEditDto>();
            CreateMap<Email, EmailCreateDto>().ReverseMap();
            CreateMap<Email, EmailDeleteDto>();
            CreateMap<Email, EmailEditDto>();
        }
    }
}
