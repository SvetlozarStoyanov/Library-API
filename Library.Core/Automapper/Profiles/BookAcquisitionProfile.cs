using AutoMapper;
using Library.Core.Dto.BookAcquisitions;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class BookAcquisitionProfile : Profile
    {
        public BookAcquisitionProfile()
        {
            CreateMap<BookAcquisition, BookAcquisitionListDto>()
                .ForMember(ba => ba.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(ba => ba.Time, opt => opt.MapFrom(src => src.Time.ToString()));

            CreateMap<BookAcquisition, BookAcquisitionRestockDto>().ReverseMap();
        }
    }
}
