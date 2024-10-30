using AutoMapper;
using Library.Core.Automapper.Profiles;

namespace Library.Core.Automapper.Config
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings(IMapperConfigurationExpression config)
        {
            config.AddProfile<AddressProfile>();
            config.AddProfile<AuthorProfile>();
            config.AddProfile<BookAcquisitionProfile>();
            config.AddProfile<BookProfile>();
            config.AddProfile<CheckoutProfile>();
            config.AddProfile<ClientCardProfile>();
            config.AddProfile<ClientCardTypeProfile>();
            config.AddProfile<ClientCardStatusChangeProfile>();
            config.AddProfile<ClientProfile>();
            config.AddProfile<CountryProfile>();
            config.AddProfile<EmailProfile>();
            config.AddProfile<FineProfile>();
            config.AddProfile<GenreProfile>();
            config.AddProfile<LanguageProfile>();
            config.AddProfile<PhoneNumberProfile>();
            config.AddProfile<SeriesProfile>();
        }
    }
}
