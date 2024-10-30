using Library.Infrastructure.DataAccess.Contracts;

namespace Library.Infrastructure.DataPersistence.Contracts
{
    public interface IUnitOfWork
    {
        #region Repository getters
        IBookRepository BookRepository { get; }
        IBookAcquisitionRepository BookAcquisitionRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        IGenreRepository GenreRepository { get; }
        ISeriesRepository SeriesRepository { get; }
        ISeriesBookRepository SeriesBookRepository { get; }
        IGenreBookRepository GenreBookRepository { get; }
        IAuthorBookRepository AuthorBookRepository { get; }
        IAddressRepository AddressRepository { get; }
        ILanguageRepository LanguageRepository { get; }
        ICountryRepository CountryRepository { get; }
        IPhoneNumberRepository PhoneNumberRepository { get; }
        IEmailRepository EmailRepository { get; }
        IFineRepository FineRepository { get; }
        ICheckoutRepository CheckoutRepository { get; }
        IClientRepository ClientRepository { get; }
        IClientCardRepository ClientCardRepository { get; }
        IClientCardTypeRepository ClientCardTypeRepository { get; }
        IClientCardStatusChangeRepository ClientCardStatusChangeRepository { get; }
        #endregion
        /// <summary>
        /// Saves all made changes in transaction
        /// </summary>
        /// <returns>Error code</returns>
        Task<int> SaveChangesAsync();
    }
}
