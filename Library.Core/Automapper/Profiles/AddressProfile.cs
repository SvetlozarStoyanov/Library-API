using AutoMapper;
using Library.Core.Dto.Addresses;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressListDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<Address, AddressEditDto>();

            CreateMap<Address, AddressCreateOrEditDto>();

            CreateMap<AddressCreateDto, Address>();
        }
    }
}
