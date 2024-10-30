using Library.Infrastructure.DataAccess.CachedRepositories;
using Library.Infrastructure.DataAccess.Contracts;
using Library.Infrastructure.DataAccess.Repositories;
using Library.Infrastructure.DataPersistence.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Library.Infrastructure.DataPersistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext dbContext;
        private readonly IMemoryCache cache;
        #region Repository fields
        private IBookRepository bookRepository;
        private IBookAcquisitionRepository bookAcquisitionRepository;
        private IAuthorRepository authorRepository;
        private IGenreRepository genreRepository;
        private ISeriesRepository seriesRepository;
        private ISeriesBookRepository seriesBookRepository;
        private IGenreBookRepository genreBookRepository;
        private IAuthorBookRepository authorBookRepository;
        private IAddressRepository addressRepository;
        private ILanguageRepository languageRepository;
        private ICountryRepository countryRepository;
        private IPhoneNumberRepository phoneNumberRepository;
        private IEmailRepository emailRepository;
        private IFineRepository fineRepository;
        private ICheckoutRepository checkoutRepository;
        private IClientRepository clientRepository;
        private IClientCardRepository clientCardRepository;
        private IClientCardTypeRepository clientCardTypeRepository;
        private IClientCardStatusChangeRepository clientCardStatusChangeRepository;

        #endregion

        public UnitOfWork(LibraryDbContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        #region Repository getters
        public IBookRepository BookRepository => bookRepository ??= new BookCachedRepository(dbContext, cache);
        public IBookAcquisitionRepository BookAcquisitionRepository => bookAcquisitionRepository ??= new BookAcquisitionRepository(dbContext);
        public IAuthorRepository AuthorRepository => authorRepository ??= new AuthorCachedRepository(dbContext, cache);
        public IGenreRepository GenreRepository => genreRepository ??= new GenreCachedRepository(dbContext, cache);
        public ISeriesRepository SeriesRepository => seriesRepository ??= new SeriesCachedRepository(dbContext, cache);
        public ISeriesBookRepository SeriesBookRepository => seriesBookRepository ??= new SeriesBookRepository(dbContext);
        public IGenreBookRepository GenreBookRepository => genreBookRepository ??= new GenreBookRepository(dbContext);
        public IAuthorBookRepository AuthorBookRepository => authorBookRepository ??= new AuthorBookRepository(dbContext);
        public IAddressRepository AddressRepository => addressRepository ??= new AddressRepository(dbContext);
        public ILanguageRepository LanguageRepository => languageRepository ??= new LanguageCachedRepository(dbContext, cache);
        public ICountryRepository CountryRepository => countryRepository ??= new CountryCachedRepository(dbContext, cache);
        public IPhoneNumberRepository PhoneNumberRepository => phoneNumberRepository ??= new PhoneNumberRepository(dbContext);
        public IEmailRepository EmailRepository => emailRepository ??= new EmailRepository(dbContext);
        public IFineRepository FineRepository => fineRepository ??= new FineRepository(dbContext);
        public ICheckoutRepository CheckoutRepository => checkoutRepository ??= new CheckoutRepository(dbContext);
        public IClientRepository ClientRepository => clientRepository ??= new ClientRepository(dbContext);
        public IClientCardRepository ClientCardRepository => clientCardRepository ??= new ClientCardRepository(dbContext);
        public IClientCardTypeRepository ClientCardTypeRepository => clientCardTypeRepository ??= new ClientCardTypeRepository(dbContext);
        public IClientCardStatusChangeRepository ClientCardStatusChangeRepository => clientCardStatusChangeRepository ??= new ClientCardStatusChangeRepository(dbContext);
        #endregion

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
