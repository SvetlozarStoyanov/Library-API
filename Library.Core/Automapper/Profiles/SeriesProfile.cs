using AutoMapper;
using Library.Core.Dto.Series;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class SeriesProfile : Profile
    {
        public SeriesProfile()
        {
            CreateMap<Series, SeriesDetailsDto>();
            CreateMap<Series, SeriesListDto>();
            CreateMap<Series, SeriesSelectDto>();
            CreateMap<Series, SeriesCreateDto>().ReverseMap();
            CreateMap<Series, SeriesEditDto>();
            CreateMap<Series, SeriesNestedListDto>();
        }
    }
}
