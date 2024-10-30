using AutoMapper;
using Library.Core.Dto.Clients;
using Library.Infrastructure.Entities;

namespace Library.Core.Automapper.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientListDto>()
                .ForMember(cl => cl.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {(src.MiddleName != null ? $"{src.MiddleName} " : "")}{src.LastName}"));

            CreateMap<Client, ClientDetailsDto>();

            CreateMap<Client, ClientAddressesEditDto>()
                .ForMember(cl => cl.ClientId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Client, ClientEmailsEditDto>()
                .ForMember(cl => cl.ClientId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Client, ClientPhoneNumbersEditDto>()
                .ForMember(cl => cl.ClientId, opt => opt.MapFrom(src => src.Id))
                .ForMember(cl => cl.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers));

            CreateMap<Client, ClientNestedListDto>()
                .ForMember(cl => cl.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {(src.MiddleName != null ? $"{src.MiddleName} " : "")}{src.LastName}"));

            CreateMap<Client, ClientEditDto>();

            CreateMap<ClientCreateDto, Client>()
                .ForMember(cl => cl.Code, opt => opt.Ignore())
                .ForMember(cl => cl.Addresses, opt => opt.Ignore())
                .ForMember(cl => cl.Emails, opt => opt.Ignore())
                .ForMember(cl => cl.PhoneNumbers, opt => opt.Ignore());

        }
    }
}
