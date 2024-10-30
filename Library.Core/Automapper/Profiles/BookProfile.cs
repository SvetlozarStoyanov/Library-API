using AutoMapper;
using Library.Core.Dto.Books;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookListDto>();
            CreateMap<Book, BookDetailsDto>();

            CreateMap<Book, BookEditDto>();

            CreateMap<Book, BookExperimentalCreateDto>();
            CreateMap<Book, BookCreateDto>();
            CreateMap<Book, BookNestedListDto>();

            CreateMap<BookCreateDto, Book>();

            CreateMap<BookExperimentalCreateDto, Book>()
                .ForMember(b => b.Authors, opt => opt.Ignore())
                .ForMember(b => b.Genres, opt => opt.Ignore())
                .ForMember(b => b.Series, opt => opt.Ignore());
        }
    }
}
