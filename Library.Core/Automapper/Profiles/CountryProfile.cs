using AutoMapper;
using Library.Core.Dto.Countries;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryNameDto>();
            CreateMap<Country, CountryListDto>();
            CreateMap<Country, CountrySelectDto>();
        }
    }
}
