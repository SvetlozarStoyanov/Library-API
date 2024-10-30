using AutoMapper;
using Library.Core.Dto.Authors;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorListDto>()
                .ForMember(a => a.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {(src.MiddleName != null ? $"{src.MiddleName} " : "")}{src.LastName}"));

            CreateMap<Author, AuthorDetailsDto>();
            CreateMap<Author, AuthorEditDto>();

            CreateMap<Author, AuthorNestedListDto>()
                .ForMember(a => a.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {(src.MiddleName != null ? $"{src.MiddleName} " : "")}{src.LastName}"));

            CreateMap<AuthorCreateDto, Author>();

            CreateMap<Author, AuthorSelectDto>()
                .ForMember(a => a.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {(src.MiddleName != null ? $"{src.MiddleName} " : "")}{src.LastName}"));
        }
    }
}
