using AutoMapper;
using Library.Core.Dto.Checkouts;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class CheckoutProfile : Profile
    {
        public CheckoutProfile()
        {
            CreateMap<Checkout, CheckoutListDto>();
            CreateMap<Checkout, CheckoutCreateDto>();
            CreateMap<Checkout, CheckoutDetailsDto>();
            CreateMap<Checkout, CheckoutFinalizationDto>();
        }
    }
}
