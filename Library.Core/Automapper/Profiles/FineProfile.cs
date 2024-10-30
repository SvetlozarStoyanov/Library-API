using AutoMapper;
using Library.Core.Dto.Fines;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class FineProfile : Profile
    {
        public FineProfile()
        {
            CreateMap<Fine, FineListDto>()
                .ForMember(f => f.Reason, opt => opt.MapFrom(src => src.Reason.ToString()))
                .ForMember(f => f.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Fine, FineAdjustmentDto>();
            CreateMap<Fine, FineCreateDto>();
            CreateMap<Fine, FineWaiverDto>();
            CreateMap<Fine, FinePaymentDto>();


            CreateMap<FineCreateDto, Fine>()
                .ForMember(f => f.Status, opt => opt.Ignore())
                .ForMember(f => f.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(f => f.IssueDate, opt => opt.MapFrom(src => src.Date));
        }
    }
}
