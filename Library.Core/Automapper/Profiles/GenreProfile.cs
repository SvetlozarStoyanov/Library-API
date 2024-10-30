using AutoMapper;
using Library.Core.Dto.Genres;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreListDto>();
            CreateMap<Genre, GenreSelectDto>();
            CreateMap<Genre, GenreCreateDto>().ReverseMap();
            CreateMap<Genre, GenreEditDto>();
            CreateMap<Genre, GenreDetailsDto>();
            CreateMap<Genre, GenreNestedListDto>();
        }
    }
}
