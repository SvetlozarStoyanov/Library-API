using AutoMapper;
using Library.Core.Dto.PhoneNumbers;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class PhoneNumberProfile : Profile
    {
        public PhoneNumberProfile()
        {
            CreateMap<PhoneNumber, PhoneNumberListDto>()
                .ForMember(pn => pn.Type, opt => opt.MapFrom(src => src.Type.ToString()));


            CreateMap<PhoneNumber, PhoneNumberCreateDto>().ReverseMap();
            CreateMap<PhoneNumber, PhoneNumberDeleteDto>();
            CreateMap<PhoneNumber, PhoneNumberEditDto>();

            CreateMap<PhoneNumber, PhoneNumberCreateOrEditDto>();
        }
    }
}
