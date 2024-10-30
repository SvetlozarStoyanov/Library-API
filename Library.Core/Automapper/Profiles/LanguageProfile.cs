using AutoMapper;
using Library.Core.Dto.Languages;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class LanguageProfile : Profile
    {
        public LanguageProfile()
        {
            CreateMap<Language, LanguageCreateDto>().ReverseMap();
            CreateMap<Language, LanguageListDto>();
            CreateMap<Language, LanguageSelectDto>();
            CreateMap<Language, LanguageEditDto>();
        }
    }
}
