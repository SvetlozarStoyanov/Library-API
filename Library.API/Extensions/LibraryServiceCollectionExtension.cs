using Library.API.Middlewares;
using Library.Core.Automapper.Config;
using Library.Core.Contracts.DbServices;
using Library.Core.Contracts.HelperServices;
using Library.Core.Services.DbServices;
using Library.Core.Services.HelperServices;
using Library.Infrastructure.DataPersistence.Contracts;
using Library.Infrastructure.DataPersistence.UnitOfWork;
using Library.Infrastructure.DataSeeding.Contracts;
using Library.Infrastructure.DataSeeding.Seeder;

namespace Library.API.Extensions
{
    public static class LibraryServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            AddDataAccessLayerServices(services);
            AddCustomMiddlewares(services);
            AddServiceLayerServices(services);
            AddAutoMapper(services);
            AddCaching(services);
            AddSeedingServices(services);
            return services;
        }

        private static void AddDataAccessLayerServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddCustomMiddlewares(IServiceCollection services)
        {
            services.AddTransient<GlobalExceptionHandlingMiddleware>();
        }

        private static void AddServiceLayerServices(IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookAcquisitionService, BookAcquisitionService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPhoneNumberService, PhoneNumberService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ISeriesService, SeriesService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientCardService, ClientCardService>();
            services.AddScoped<IClientCardStatusChangeService, ClientCardStatusChangeService>();
            services.AddScoped<IClientCardTypeService, ClientCardTypeService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IFineService, FineService>();
            services.AddScoped<IAuthorBookService, AuthorBookService>();
            services.AddScoped<IGenreBookService, GenreBookService>();
            services.AddScoped<ISeriesBookService, SeriesBookService>();

            services.AddScoped<IEnumService, EnumService>();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                AutoMapperConfig.RegisterMappings(cfg);
                cfg.DisableConstructorMapping();
            });
        }

        private static void AddCaching(IServiceCollection services)
        {
            services.AddMemoryCache();
        }

        private static void AddSeedingServices(IServiceCollection services)
        {
            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
            services.AddScoped<ISeedingService, SeedingService>();
        }
    }
}
