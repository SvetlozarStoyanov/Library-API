using Library.Infrastructure.DataSeeding.Contracts;
using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.DataSeeding.Seeder
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly LibraryDbContext context;
        private IList<Country> countries;
        private IList<Language> languages;
        private IList<Author> authors;
        private IList<Genre> genres;
        private IList<Series> series;
        private IList<Email> emails;
        private IList<Checkout> checkouts;
        private IList<PhoneNumber> phoneNumbers;
        private IList<Book> books;
        private IList<BookAcquisition> bookAcquisitions;
        private IList<ClientCardType> clientCardTypes;
        private IList<Client> clients;
        private IList<ClientCard> clientCards;
        private IList<ClientCardStatusChange> clientCardStatusChanges;
        private IList<Address> addresses;
        private IList<Fine> fines;
        private IList<AuthorBook> authorBooks;
        private IList<GenreBook> genreBooks;
        private IList<SeriesBook> seriesBooks;

        public DatabaseSeeder(LibraryDbContext libraryDbContext)
        {
            context = libraryDbContext;
        }

        public async Task<bool> CheckDatabaseIsSeededAsync()
        {
            return await context.Set<Seeding>().AnyAsync();
        }

        public async Task SeedAsync()
        {
            if (await CheckDatabaseIsSeededAsync())
            {
                throw new InvalidOperationException("Database is already seeded!");
            }

            try
            {
                await context.AddRangeAsync(CreateAuthors());
                await context.AddRangeAsync(CreateSeries());
                await context.AddRangeAsync(CreateGenres());
                await context.AddRangeAsync(CreateCountries());
                await context.AddRangeAsync(CreateClientCardTypes());
                await context.AddRangeAsync(CreateLanguages());

                await context.AddRangeAsync(CreateClients());
                await context.AddRangeAsync(CreateBooks());

                await context.AddRangeAsync(CreateClientCards());
                await context.AddRangeAsync(CreateEmails());
                await context.AddRangeAsync(CreatePhoneNumbers());
                await context.AddRangeAsync(CreateAddresses());

                await context.AddRangeAsync(CreateClientCardStatusChanges());

                await context.AddRangeAsync(CreateBookAcquisitions());
                await context.AddRangeAsync(CreateGenreBooks());
                await context.AddRangeAsync(CreateAuthorBooks());
                await context.AddRangeAsync(CreateSeriesBooks());
                await context.AddRangeAsync(CreateCheckouts());

                await context.AddRangeAsync(CreateFines());
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Transaction error!");
            }

            Seeding seeding = new Seeding();
            seeding.Date = DateTime.Now;

            await context.AddAsync(seeding);
            await context.SaveChangesAsync();
        }

        #region EntityCreation
        private IEnumerable<Address> CreateAddresses()
        {
            addresses = new List<Address>()
            {
                new Address { City = "Varna", AddressLine = "Pirin 10", PostalCode = "9000", Type = AddressType.Residency, Client = clients[0], Country = countries[25] },
                new Address { City = "Varna", AddressLine = "Pirin 15", PostalCode = "9000", Type = AddressType.Residency, Client = clients[1], Country = countries[25] },
                new Address { City = "Varna", AddressLine = "Batova 13", PostalCode = "9000", Type = AddressType.Residency, Client = clients[2], Country = countries[25] },
                new Address { City = "Varna", AddressLine = "Bdin 6", PostalCode = "9000", Type = AddressType.Residency, Client = clients[3], Country = countries[25] },
                new Address { City = "Varna", AddressLine = "Bdin 18", PostalCode = "9000", Type = AddressType.Residency, Client = clients[4], Country = countries[25] }
            };

            return addresses;
        }

        private IEnumerable<AuthorBook> CreateAuthorBooks()
        {
            authorBooks = new List<AuthorBook>()
            {
                new AuthorBook { Author = authors[2], Book = books[0] },
                new AuthorBook { Author = authors[2], Book = books[1] },
                new AuthorBook { Author = authors[2], Book = books[2] },
                new AuthorBook { Author = authors[0], Book = books[3] },
                new AuthorBook { Author = authors[0], Book = books[4] },
                new AuthorBook { Author = authors[0], Book = books[5] },
                new AuthorBook { Author = authors[0], Book = books[6] },
                new AuthorBook { Author = authors[0], Book = books[7] },
                new AuthorBook { Author = authors[1], Book = books[8] },
                new AuthorBook { Author = authors[1], Book = books[9] },
                new AuthorBook { Author = authors[1], Book = books[10] }
            };

            return authorBooks;
        }

        private IEnumerable<Author> CreateAuthors()
        {
            authors = new List<Author>()
            {
                new Author { FirstName = "George", MiddleName = "R.R.", LastName = "Martin", Description = "Description", DateOfBirth = new DateTime(1948, 9, 20), DateOfDeath = null },
                new Author { FirstName = "John", MiddleName = "R.R.", LastName = "Tolkien", Description = "Description", DateOfBirth = new DateTime(1892, 1, 3), DateOfDeath = new DateTime(1973, 9, 2) },
                new Author { FirstName = "Joanne", MiddleName = "Kathleen", LastName = "Rowling", Description = "Description", DateOfBirth = new DateTime(1965, 7, 31), DateOfDeath = null }
            };

            return authors;
        }

        private IEnumerable<Book> CreateBooks()
        {
            books = new List<Book>()
            {
                new Book { Title = "Harry Potter and the Philosopher's Stone", ISBN = "9781566199094", Description = "Description", PageCount = 223, Quantity = 100, PublicationDate = new DateTime(1994, 6, 26), Language = languages[0], Country = countries[180] },
                new Book { Title = "Harry Potter and the Chamber of Secrets", ISBN = "9781234567897", Description = "Description", PageCount = 352, Quantity = 60, PublicationDate = new DateTime(1998, 7, 2), Language = languages[0], Country = countries[180] },
                new Book { Title = "Harry Potter and the Prisoner of Azkaban", ISBN = "9781566199194", Description = "Description", PageCount = 400, Quantity = 50, PublicationDate = new DateTime(2001, 6, 15), Language = languages[0], Country = countries[180] },
                new Book { Title = "A Game of Thrones", ISBN = "9781562198194", Description = "Description", PageCount = 694, Quantity = 128, PublicationDate = new DateTime(1996, 8, 6), Language = languages[0], Country = countries[181] },
                new Book { Title = "A Clash of Kings", ISBN = "6781562198194", Description = "Description", PageCount = 761, Quantity = 111, PublicationDate = new DateTime(1998, 11, 16), Language = languages[0], Country = countries[181] },
                new Book { Title = "A Storm of Swords", ISBN = "1781562168194", Description = "Description", PageCount = 973, Quantity = 89, PublicationDate = new DateTime(2000, 8, 8), Language = languages[0], Country = countries[181] },
                new Book { Title = "A Feast for Crows", ISBN = "0002247437", Description = "Description", PageCount = 753, Quantity = 68, PublicationDate = new DateTime(2005, 10, 17), Language = languages[0], Country = countries[181] },
                new Book { Title = "A Dance with Dragons", ISBN = "9780007456376", Description = "Description", PageCount = 1016, Quantity = 59, PublicationDate = new DateTime(2011, 7, 12), Language = languages[0], Country = countries[181] },
                new Book { Title = "The Fellowship of the Ring", ISBN = null, Description = "Description", PageCount = 423, Quantity = 77, PublicationDate = new DateTime(1954, 7, 29), Language = languages[0], Country = countries[180] },
                new Book { Title = "The Two Towers", ISBN = null, Description = "Description", PageCount = 352, Quantity = 66, PublicationDate = new DateTime(1954, 11, 11), Language = languages[0], Country = countries[180] },
                new Book { Title = "The Return of the King", ISBN = null, Description = "Description", PageCount = 416, Quantity = 55, PublicationDate = new DateTime(1955, 10, 20), Language = languages[0], Country = countries[180] }
            };

            return books;
        }

        private IEnumerable<BookAcquisition> CreateBookAcquisitions()
        {
            bookAcquisitions = new List<BookAcquisition>()
            {
                new BookAcquisition { Quantity = 100, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[0] },
                new BookAcquisition { Quantity = 60, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[1] },
                new BookAcquisition { Quantity = 50, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[2] },
                new BookAcquisition { Quantity = 128, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[3]},
                new BookAcquisition { Quantity = 111, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[4] },
                new BookAcquisition { Quantity = 89, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[5] },
                new BookAcquisition { Quantity = 68, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[6] },
                new BookAcquisition { Quantity = 59, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[7] },
                new BookAcquisition { Quantity = 77, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[8] },
                new BookAcquisition { Quantity = 66, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[9] },
                new BookAcquisition { Quantity = 55, Time = DateTime.Now.AddYears(-2), Type = BookAcquisitionType.Initial, Book = books[10] }
            };

            return bookAcquisitions;
        }

        private IEnumerable<Checkout> CreateCheckouts()
        {
            checkouts = new List<Checkout>()
            {
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-2), DueTime = DateTime.Now.AddDays(21), ReturnTime = null, ClientCard = clientCards[0], Book = books[2] },
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-2), DueTime = DateTime.Now.AddDays(21), ReturnTime = null, ClientCard = clientCards[0], Book = books[1] },
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-2), DueTime = DateTime.Now.AddDays(21), ReturnTime = null, ClientCard = clientCards[0], Book = books[0] },
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-3), DueTime = DateTime.Now.AddDays(31), ReturnTime = null, ClientCard = clientCards[1], Book = books[7] },
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-3), DueTime = DateTime.Now.AddDays(31), ReturnTime = null, ClientCard = clientCards[1], Book = books[6] },
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-3), DueTime = DateTime.Now.AddDays(31), ReturnTime = null, ClientCard = clientCards[1], Book = books[5] },
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-3), DueTime = DateTime.Now.AddDays(31), ReturnTime = null, ClientCard = clientCards[1], Book = books[4] },
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-3), DueTime = DateTime.Now.AddDays(31), ReturnTime = null, ClientCard = clientCards[1], Book = books[3] },
                new Checkout { CheckoutTime = DateTime.Now.AddDays(-26), DueTime = DateTime.Now.AddDays(-5), ReturnTime = null, ClientCard = clientCards[2], Book = books[3] }
            };

            return checkouts;
        }

        private IEnumerable<ClientCard> CreateClientCards()
        {
            clientCards = new List<ClientCard>()
            {
                new ClientCard { Status = ClientCardStatus.Active, CreationDate = new DateTime(2024, 6, 21), ExpirationDate = new DateTime(2024, 6, 21).AddYears(1), Client = clients[0], ClientCardType = clientCardTypes[0] },
                new ClientCard { Status = ClientCardStatus.Active, CreationDate = new DateTime(2024, 6, 21), ExpirationDate = new DateTime(2024, 6, 21).AddYears(1), Client = clients[1], ClientCardType = clientCardTypes[1] },
                new ClientCard { Status = ClientCardStatus.Suspended, CreationDate = new DateTime(2024, 6, 21), ExpirationDate = new DateTime(2024, 6, 21).AddYears(1), Client = clients[2], ClientCardType = clientCardTypes[0] },
                new ClientCard { Status = ClientCardStatus.Active, CreationDate = new DateTime(2024, 6, 21), ExpirationDate = new DateTime(2024, 6, 21).AddYears(1), Client = clients[3], ClientCardType = clientCardTypes[0] },
                new ClientCard { Status = ClientCardStatus.Active, CreationDate = new DateTime(2024, 6, 21), ExpirationDate = new DateTime(2024, 6, 21).AddYears(1), Client = clients[4], ClientCardType = clientCardTypes[0] },
            };

            return clientCards;
        }

        private IEnumerable<ClientCardType> CreateClientCardTypes()
        {
            clientCardTypes = new List<ClientCardType>()
            {
                new ClientCardType { Name = "Standart", CheckoutQuantityLimit = 5, CheckoutTimeLimit = 21, MonthsValid = 12 },
                new ClientCardType { Name = "Educator", CheckoutQuantityLimit = 100, CheckoutTimeLimit = 31, MonthsValid = 12 }
            };

            return clientCardTypes;
        }

        private IEnumerable<ClientCardStatusChange> CreateClientCardStatusChanges()
        {
            clientCardStatusChanges = new List<ClientCardStatusChange>()
            {
                new ClientCardStatusChange { UpdatedOn = new DateTime(2024, 6, 21), Reason = ClientCardStatusChangeReason.InitialActivation, Status = ClientCardStatus.Active, ClientCard = clientCards[0] },
                new ClientCardStatusChange { UpdatedOn = new DateTime(2024, 6, 21), Reason = ClientCardStatusChangeReason.InitialActivation, Status = ClientCardStatus.Active, ClientCard = clientCards[1] },
                new ClientCardStatusChange { UpdatedOn = new DateTime(2024, 6, 21), Reason = ClientCardStatusChangeReason.InitialActivation, Status = ClientCardStatus.Active, ClientCard = clientCards[2] },
                new ClientCardStatusChange { UpdatedOn = new DateTime(2024, 6, 21), Reason = ClientCardStatusChangeReason.InitialActivation, Status = ClientCardStatus.Active, ClientCard = clientCards[3] },
                new ClientCardStatusChange { UpdatedOn = new DateTime(2024, 6, 21), Reason = ClientCardStatusChangeReason.InitialActivation, Status = ClientCardStatus.Active, ClientCard = clientCards[4] },
                new ClientCardStatusChange { UpdatedOn = new DateTime(2024, 7, 2), Reason = ClientCardStatusChangeReason.Suspension, Status = ClientCardStatus.Suspended, ClientCard = clientCards[2] }
            };

            return clientCardStatusChanges;
        }

        private IEnumerable<Client> CreateClients()
        {
            clients = new List<Client>()
            {
                new Client { FirstName = "Georgi", MiddleName = null, LastName = "Petrov", Code = "AT-457662", UnifiedCivilNumber = "9605126542", DateOfBirth = new DateTime(1996, 5, 12) },
                new Client { FirstName = "Kamen", MiddleName = null, LastName = "Todorov", Code = "CP-152851", UnifiedCivilNumber = "9104062122", DateOfBirth = new DateTime(1991, 4, 6) },
                new Client { FirstName = "Hristian", MiddleName = null, LastName = "Georgiev", Code = "HF-442134", UnifiedCivilNumber = "9906116847", DateOfBirth = new DateTime(1999, 6, 11) },
                new Client { FirstName = "Teodor", MiddleName = null, LastName = "Trendafilov", Code = "FO-327548", UnifiedCivilNumber = "8606124679", DateOfBirth = new DateTime(1986, 6, 12) },
                new Client { FirstName = "Vladislav", MiddleName = null, LastName = "Ivanov", Code = "TW-857177", UnifiedCivilNumber = "9802187821", DateOfBirth = new DateTime(1998, 2, 18) }
            };

            return clients;
        }

        private IEnumerable<Country> CreateCountries()
        {
            countries = new List<Country>()
            {
                new Country { Name = "Afghanistan", IsoCodeTwo = "AF", IsoCodeThree = "AFG", PhoneCode = "+93" },
                new Country { Name = "Albania", IsoCodeTwo = "AL", IsoCodeThree = "ALB", PhoneCode = "+355" },
                new Country { Name = "Algeria", IsoCodeTwo = "DZ", IsoCodeThree = "DZA", PhoneCode = "+213" },
                new Country { Name = "Andorra", IsoCodeTwo = "AD", IsoCodeThree = "AND", PhoneCode = "+376" },
                new Country { Name = "Angola", IsoCodeTwo = "AO", IsoCodeThree = "AGO", PhoneCode = "+244" },
                new Country { Name = "Antigua and Barbuda", IsoCodeTwo = "AG", IsoCodeThree = "ATG", PhoneCode = "+1-268" },
                new Country { Name = "Argentina", IsoCodeTwo = "AR", IsoCodeThree = "ARG", PhoneCode = "+54" },
                new Country { Name = "Armenia", IsoCodeTwo = "AM", IsoCodeThree = "ARM", PhoneCode = "+374" },
                new Country { Name = "Australia", IsoCodeTwo = "AU", IsoCodeThree = "AUS", PhoneCode = "+61" },
                new Country { Name = "Austria", IsoCodeTwo = "AT", IsoCodeThree = "AUT", PhoneCode = "+43" },
                new Country { Name = "Azerbaijan", IsoCodeTwo = "AZ", IsoCodeThree = "AZE", PhoneCode = "+994" },
                new Country { Name = "Bahamas", IsoCodeTwo = "BS", IsoCodeThree = "BHS", PhoneCode = "+1-242" },
                new Country { Name = "Bahrain", IsoCodeTwo = "BH", IsoCodeThree = "BHR", PhoneCode = "+973" },
                new Country { Name = "Bangladesh", IsoCodeTwo = "BD", IsoCodeThree = "BGD", PhoneCode = "+880" },
                new Country { Name = "Barbados", IsoCodeTwo = "BB", IsoCodeThree = "BRB", PhoneCode = "+1-246" },
                new Country { Name = "Belarus", IsoCodeTwo = "BY", IsoCodeThree = "BLR", PhoneCode = "+375" },
                new Country { Name = "Belgium", IsoCodeTwo = "BE", IsoCodeThree = "BEL", PhoneCode = "+32" },
                new Country { Name = "Belize", IsoCodeTwo = "BZ", IsoCodeThree = "BLZ", PhoneCode = "+501" },
                new Country { Name = "Benin", IsoCodeTwo = "BJ", IsoCodeThree = "BEN", PhoneCode = "+229" },
                new Country { Name = "Bhutan", IsoCodeTwo = "BT", IsoCodeThree = "BTN", PhoneCode = "+975" },
                new Country { Name = "Bolivia", IsoCodeTwo = "BO", IsoCodeThree = "BOL", PhoneCode = "+591" },
                new Country { Name = "Bosnia and Herzegovina", IsoCodeTwo = "BA", IsoCodeThree = "BIH", PhoneCode = "+387" },
                new Country { Name = "Botswana", IsoCodeTwo = "BW", IsoCodeThree = "BWA", PhoneCode = "+267" },
                new Country { Name = "Brazil", IsoCodeTwo = "BR", IsoCodeThree = "BRA", PhoneCode = "+55" },
                new Country { Name = "Brunei Darussalam", IsoCodeTwo = "BN", IsoCodeThree = "BRN", PhoneCode = "+673" },
                new Country { Name = "Bulgaria", IsoCodeTwo = "BG", IsoCodeThree = "BGR", PhoneCode = "+359" },
                new Country { Name = "Burkina Faso", IsoCodeTwo = "BF", IsoCodeThree = "BFA", PhoneCode = "+226" },
                new Country { Name = "Burundi", IsoCodeTwo = "BI", IsoCodeThree = "BDI", PhoneCode = "+257" },
                new Country { Name = "Cabo Verde", IsoCodeTwo = "CV", IsoCodeThree = "CPV", PhoneCode = "+238" },
                new Country { Name = "Cambodia", IsoCodeTwo = "KH", IsoCodeThree = "KHM", PhoneCode = "+855" },
                new Country { Name = "Cameroon", IsoCodeTwo = "CM", IsoCodeThree = "CMR", PhoneCode = "+237" },
                new Country { Name = "Canada", IsoCodeTwo = "CA", IsoCodeThree = "CAN", PhoneCode = "+1" },
                new Country { Name = "Central African Republic", IsoCodeTwo = "CF", IsoCodeThree = "CAF", PhoneCode = "+236" },
                new Country { Name = "Chad", IsoCodeTwo = "TD", IsoCodeThree = "TCD", PhoneCode = "+235" },
                new Country { Name = "Chile", IsoCodeTwo = "CL", IsoCodeThree = "CHL", PhoneCode = "+56" },
                new Country { Name = "China", IsoCodeTwo = "CN", IsoCodeThree = "CHN", PhoneCode = "+86" },
                new Country { Name = "Colombia", IsoCodeTwo = "CO", IsoCodeThree = "COL", PhoneCode = "+57" },
                new Country { Name = "Comoros", IsoCodeTwo = "KM", IsoCodeThree = "COM", PhoneCode = "+269" },
                new Country { Name = "Congo", IsoCodeTwo = "CG", IsoCodeThree = "COG", PhoneCode = "+242" },
                new Country { Name = "Congo, Democratic Republic of the", IsoCodeTwo = "CD", IsoCodeThree = "COD", PhoneCode = "+243" },
                new Country { Name = "Costa Rica", IsoCodeTwo = "CR", IsoCodeThree = "CRI", PhoneCode = "+506" },
                new Country { Name = "Croatia", IsoCodeTwo = "HR", IsoCodeThree = "HRV", PhoneCode = "+385" },
                new Country { Name = "Cuba", IsoCodeTwo = "CU", IsoCodeThree = "CUB", PhoneCode = "+53" },
                new Country { Name = "Cyprus", IsoCodeTwo = "CY", IsoCodeThree = "CYP", PhoneCode = "+357" },
                new Country { Name = "Czech Republic", IsoCodeTwo = "CZ", IsoCodeThree = "CZE", PhoneCode = "+420" },
                new Country { Name = "Denmark", IsoCodeTwo = "DK", IsoCodeThree = "DNK", PhoneCode = "+45" },
                new Country { Name = "Djibouti", IsoCodeTwo = "DJ", IsoCodeThree = "DJI", PhoneCode = "+253" },
                new Country { Name = "Dominica", IsoCodeTwo = "DM", IsoCodeThree = "DMA", PhoneCode = "+1-767" },
                new Country { Name = "Dominican Republic", IsoCodeTwo = "DO", IsoCodeThree = "DOM", PhoneCode = "+1-809" },
                new Country { Name = "Ecuador", IsoCodeTwo = "EC", IsoCodeThree = "ECU", PhoneCode = "+593" },
                new Country { Name = "Egypt", IsoCodeTwo = "EG", IsoCodeThree = "EGY", PhoneCode = "+20" },
                new Country { Name = "El Salvador", IsoCodeTwo = "SV", IsoCodeThree = "SLV", PhoneCode = "+503" },
                new Country { Name = "Equatorial Guinea", IsoCodeTwo = "GQ", IsoCodeThree = "GNQ", PhoneCode = "+240" },
                new Country { Name = "Eritrea", IsoCodeTwo = "ER", IsoCodeThree = "ERI", PhoneCode = "+291" },
                new Country { Name = "Estonia", IsoCodeTwo = "EE", IsoCodeThree = "EST", PhoneCode = "+372" },
                new Country { Name = "Eswatini", IsoCodeTwo = "SZ", IsoCodeThree = "SWZ", PhoneCode = "+268" },
                new Country { Name = "Ethiopia", IsoCodeTwo = "ET", IsoCodeThree = "ETH", PhoneCode = "+251" },
                new Country { Name = "Fiji", IsoCodeTwo = "FJ", IsoCodeThree = "FJI", PhoneCode = "+679" },
                new Country { Name = "Finland", IsoCodeTwo = "FI", IsoCodeThree = "FIN", PhoneCode = "+358" },
                new Country { Name = "France", IsoCodeTwo = "FR", IsoCodeThree = "FRA", PhoneCode = "+33" },
                new Country { Name = "Gabon", IsoCodeTwo = "GA", IsoCodeThree = "GAB", PhoneCode = "+241" },
                new Country { Name = "Gambia", IsoCodeTwo = "GM", IsoCodeThree = "GMB", PhoneCode = "+220" },
                new Country { Name = "Georgia", IsoCodeTwo = "GE", IsoCodeThree = "GEO", PhoneCode = "+995" },
                new Country { Name = "Germany", IsoCodeTwo = "DE", IsoCodeThree = "DEU", PhoneCode = "+49" },
                new Country { Name = "Ghana", IsoCodeTwo = "GH", IsoCodeThree = "GHA", PhoneCode = "+233" },
                new Country { Name = "Greece", IsoCodeTwo = "GR", IsoCodeThree = "GRC", PhoneCode = "+30" },
                new Country { Name = "Grenada", IsoCodeTwo = "GD", IsoCodeThree = "GRD", PhoneCode = "+1-473" },
                new Country { Name = "Guatemala", IsoCodeTwo = "GT", IsoCodeThree = "GTM", PhoneCode = "+502" },
                new Country { Name = "Guinea", IsoCodeTwo = "GN", IsoCodeThree = "GIN", PhoneCode = "+224" },
                new Country { Name = "Guinea-Bissau", IsoCodeTwo = "GW", IsoCodeThree = "GNB", PhoneCode = "+245" },
                new Country { Name = "Guyana", IsoCodeTwo = "GY", IsoCodeThree = "GUY", PhoneCode = "+592" },
                new Country { Name = "Haiti", IsoCodeTwo = "HT", IsoCodeThree = "HTI", PhoneCode = "+509" },
                new Country { Name = "Honduras", IsoCodeTwo = "HN", IsoCodeThree = "HND", PhoneCode = "+504" },
                new Country { Name = "Hungary", IsoCodeTwo = "HU", IsoCodeThree = "HUN", PhoneCode = "+36" },
                new Country { Name = "Iceland", IsoCodeTwo = "IS", IsoCodeThree = "ISL", PhoneCode = "+354" },
                new Country { Name = "India", IsoCodeTwo = "IN", IsoCodeThree = "IND", PhoneCode = "+91" },
                new Country { Name = "Indonesia", IsoCodeTwo = "ID", IsoCodeThree = "IDN", PhoneCode = "+62" },
                new Country { Name = "Iran", IsoCodeTwo = "IR", IsoCodeThree = "IRN", PhoneCode = "+98" },
                new Country { Name = "Iraq", IsoCodeTwo = "IQ", IsoCodeThree = "IRQ", PhoneCode = "+964" },
                new Country { Name = "Ireland", IsoCodeTwo = "IE", IsoCodeThree = "IRL", PhoneCode = "+353" },
                new Country { Name = "Israel", IsoCodeTwo = "IL", IsoCodeThree = "ISR", PhoneCode = "+972" },
                new Country { Name = "Italy", IsoCodeTwo = "IT", IsoCodeThree = "ITA", PhoneCode = "+39" },
                new Country { Name = "Jamaica", IsoCodeTwo = "JM", IsoCodeThree = "JAM", PhoneCode = "+1-876" },
                new Country { Name = "Japan", IsoCodeTwo = "JP", IsoCodeThree = "JPN", PhoneCode = "+81" },
                new Country { Name = "Jordan", IsoCodeTwo = "JO", IsoCodeThree = "JOR", PhoneCode = "+962" },
                new Country { Name = "Kazakhstan", IsoCodeTwo = "KZ", IsoCodeThree = "KAZ", PhoneCode = "+7" },
                new Country { Name = "Kenya", IsoCodeTwo = "KE", IsoCodeThree = "KEN", PhoneCode = "+254" },
                new Country { Name = "Kiribati", IsoCodeTwo = "KI", IsoCodeThree = "KIR", PhoneCode = "+686" },
                new Country { Name = "Kuwait", IsoCodeTwo = "KW", IsoCodeThree = "KWT", PhoneCode = "+965" },
                new Country { Name = "Kyrgyzstan", IsoCodeTwo = "KG", IsoCodeThree = "KGZ", PhoneCode = "+996" },
                new Country { Name = "Lao People's Democratic Republic", IsoCodeTwo = "LA", IsoCodeThree = "LAO", PhoneCode = "+856" },
                new Country { Name = "Latvia", IsoCodeTwo = "LV", IsoCodeThree = "LVA", PhoneCode = "+371" },
                new Country { Name = "Lebanon", IsoCodeTwo = "LB", IsoCodeThree = "LBN", PhoneCode = "+961" },
                new Country { Name = "Lesotho", IsoCodeTwo = "LS", IsoCodeThree = "LSO", PhoneCode = "+266" },
                new Country { Name = "Liberia", IsoCodeTwo = "LR", IsoCodeThree = "LBR", PhoneCode = "+231" },
                new Country { Name = "Libya", IsoCodeTwo = "LY", IsoCodeThree = "LBY", PhoneCode = "+218" },
                new Country { Name = "Liechtenstein", IsoCodeTwo = "LI", IsoCodeThree = "LIE", PhoneCode = "+423" },
                new Country { Name = "Lithuania", IsoCodeTwo = "LT", IsoCodeThree = "LTU", PhoneCode = "+370" },
                new Country { Name = "Luxembourg", IsoCodeTwo = "LU", IsoCodeThree = "LUX", PhoneCode = "+352" },
                new Country { Name = "Madagascar", IsoCodeTwo = "MG", IsoCodeThree = "MDG", PhoneCode = "+261" },
                new Country { Name = "Malawi", IsoCodeTwo = "MW", IsoCodeThree = "MWI", PhoneCode = "+265" },
                new Country { Name = "Malaysia", IsoCodeTwo = "MY", IsoCodeThree = "MYS", PhoneCode = "+60" },
                new Country { Name = "Maldives", IsoCodeTwo = "MV", IsoCodeThree = "MDV", PhoneCode = "+960" },
                new Country { Name = "Mali", IsoCodeTwo = "ML", IsoCodeThree = "MLI", PhoneCode = "+223" },
                new Country { Name = "Malta", IsoCodeTwo = "MT", IsoCodeThree = "MLT", PhoneCode = "+356" },
                new Country { Name = "Marshall Islands", IsoCodeTwo = "MH", IsoCodeThree = "MHL", PhoneCode = "+692" },
                new Country { Name = "Mauritania", IsoCodeTwo = "MR", IsoCodeThree = "MRT", PhoneCode = "+222" },
                new Country { Name = "Mauritius", IsoCodeTwo = "MU", IsoCodeThree = "MUS", PhoneCode = "+230" },
                new Country { Name = "Mexico", IsoCodeTwo = "MX", IsoCodeThree = "MEX", PhoneCode = "+52" },
                new Country { Name = "Micronesia", IsoCodeTwo = "FM", IsoCodeThree = "FSM", PhoneCode = "+691" },
                new Country { Name = "Moldova", IsoCodeTwo = "MD", IsoCodeThree = "MDA", PhoneCode = "+373" },
                new Country { Name = "Monaco", IsoCodeTwo = "MC", IsoCodeThree = "MCO", PhoneCode = "+377" },
                new Country { Name = "Mongolia", IsoCodeTwo = "MN", IsoCodeThree = "MNG", PhoneCode = "+976" },
                new Country { Name = "Montenegro", IsoCodeTwo = "ME", IsoCodeThree = "MNE", PhoneCode = "+382" },
                new Country { Name = "Morocco", IsoCodeTwo = "MA", IsoCodeThree = "MAR", PhoneCode = "+212" },
                new Country { Name = "Mozambique", IsoCodeTwo = "MZ", IsoCodeThree = "MOZ", PhoneCode = "+258" },
                new Country { Name = "Myanmar", IsoCodeTwo = "MM", IsoCodeThree = "MMR", PhoneCode = "+95" },
                new Country { Name = "Namibia", IsoCodeTwo = "NA", IsoCodeThree = "NAM", PhoneCode = "+264" },
                new Country { Name = "Nauru", IsoCodeTwo = "NR", IsoCodeThree = "NRU", PhoneCode = "+674" },
                new Country { Name = "Nepal", IsoCodeTwo = "NP", IsoCodeThree = "NPL", PhoneCode = "+977" },
                new Country { Name = "Netherlands", IsoCodeTwo = "NL", IsoCodeThree = "NLD", PhoneCode = "+31" },
                new Country { Name = "New Zealand", IsoCodeTwo = "NZ", IsoCodeThree = "NZL", PhoneCode = "+64" },
                new Country { Name = "Nicaragua", IsoCodeTwo = "NI", IsoCodeThree = "NIC", PhoneCode = "+505" },
                new Country { Name = "Niger", IsoCodeTwo = "NE", IsoCodeThree = "NER", PhoneCode = "+227" },
                new Country { Name = "Nigeria", IsoCodeTwo = "NG", IsoCodeThree = "NGA", PhoneCode = "+234" },
                new Country { Name = "North Macedonia", IsoCodeTwo = "MK", IsoCodeThree = "MKD", PhoneCode = "+389" },
                new Country { Name = "Norway", IsoCodeTwo = "NO", IsoCodeThree = "NOR", PhoneCode = "+47" },
                new Country { Name = "Oman", IsoCodeTwo = "OM", IsoCodeThree = "OMN", PhoneCode = "+968" },
                new Country { Name = "Pakistan", IsoCodeTwo = "PK", IsoCodeThree = "PAK", PhoneCode = "+92" },
                new Country { Name = "Palau", IsoCodeTwo = "PW", IsoCodeThree = "PLW", PhoneCode = "+680" },
                new Country { Name = "Panama", IsoCodeTwo = "PA", IsoCodeThree = "PAN", PhoneCode = "+507" },
                new Country { Name = "Papua New Guinea", IsoCodeTwo = "PG", IsoCodeThree = "PNG", PhoneCode = "+675" },
                new Country { Name = "Paraguay", IsoCodeTwo = "PY", IsoCodeThree = "PRY", PhoneCode = "+595" },
                new Country { Name = "Peru", IsoCodeTwo = "PE", IsoCodeThree = "PER", PhoneCode = "+51" },
                new Country { Name = "Philippines", IsoCodeTwo = "PH", IsoCodeThree = "PHL", PhoneCode = "+63" },
                new Country { Name = "Poland", IsoCodeTwo = "PL", IsoCodeThree = "POL", PhoneCode = "+48" },
                new Country { Name = "Portugal", IsoCodeTwo = "PT", IsoCodeThree = "PRT", PhoneCode = "+351" },
                new Country { Name = "Qatar", IsoCodeTwo = "QA", IsoCodeThree = "QAT", PhoneCode = "+974" },
                new Country { Name = "Romania", IsoCodeTwo = "RO", IsoCodeThree = "ROU", PhoneCode = "+40" },
                new Country { Name = "Russian Federation", IsoCodeTwo = "RU", IsoCodeThree = "RUS", PhoneCode = "+7" },
                new Country { Name = "Rwanda", IsoCodeTwo = "RW", IsoCodeThree = "RWA", PhoneCode = "+250" },
                new Country { Name = "Saint Kitts and Nevis", IsoCodeTwo = "KN", IsoCodeThree = "KNA", PhoneCode = "+1-869" },
                new Country { Name = "Saint Lucia", IsoCodeTwo = "LC", IsoCodeThree = "LCA", PhoneCode = "+1-758" },
                new Country { Name = "Saint Vincent and the Grenadines", IsoCodeTwo = "VC", IsoCodeThree = "VCT", PhoneCode = "+1-784" },
                new Country { Name = "Samoa", IsoCodeTwo = "WS", IsoCodeThree = "WSM", PhoneCode = "+685" },
                new Country { Name = "San Marino", IsoCodeTwo = "SM", IsoCodeThree = "SMR", PhoneCode = "+378" },
                new Country { Name = "Sao Tome and Principe", IsoCodeTwo = "ST", IsoCodeThree = "STP", PhoneCode = "+239" },
                new Country { Name = "Saudi Arabia", IsoCodeTwo = "SA", IsoCodeThree = "SAU", PhoneCode = "+966" },
                new Country { Name = "Senegal", IsoCodeTwo = "SN", IsoCodeThree = "SEN", PhoneCode = "+221" },
                new Country { Name = "Serbia", IsoCodeTwo = "RS", IsoCodeThree = "SRB", PhoneCode = "+381" },
                new Country { Name = "Seychelles", IsoCodeTwo = "SC", IsoCodeThree = "SYC", PhoneCode = "+248" },
                new Country { Name = "Sierra Leone", IsoCodeTwo = "SL", IsoCodeThree = "SLE", PhoneCode = "+232" },
                new Country { Name = "Singapore", IsoCodeTwo = "SG", IsoCodeThree = "SGP", PhoneCode = "+65" },
                new Country { Name = "Slovakia", IsoCodeTwo = "SK", IsoCodeThree = "SVK", PhoneCode = "+421" },
                new Country { Name = "Slovenia", IsoCodeTwo = "SI", IsoCodeThree = "SVN", PhoneCode = "+386" },
                new Country { Name = "Solomon Islands", IsoCodeTwo = "SB", IsoCodeThree = "SLB", PhoneCode = "+677" },
                new Country { Name = "Somalia", IsoCodeTwo = "SO", IsoCodeThree = "SOM", PhoneCode = "+252" },
                new Country { Name = "South Africa", IsoCodeTwo = "ZA", IsoCodeThree = "ZAF", PhoneCode = "+27" },
                new Country { Name = "South Sudan", IsoCodeTwo = "SS", IsoCodeThree = "SSD", PhoneCode = "+211" },
                new Country { Name = "Spain", IsoCodeTwo = "ES", IsoCodeThree = "ESP", PhoneCode = "+34" },
                new Country { Name = "Sri Lanka", IsoCodeTwo = "LK", IsoCodeThree = "LKA", PhoneCode = "+94" },
                new Country { Name = "Sudan", IsoCodeTwo = "SD", IsoCodeThree = "SDN", PhoneCode = "+249" },
                new Country { Name = "Suriname", IsoCodeTwo = "SR", IsoCodeThree = "SUR", PhoneCode = "+597" },
                new Country { Name = "Sweden", IsoCodeTwo = "SE", IsoCodeThree = "SWE", PhoneCode = "+46" },
                new Country { Name = "Switzerland", IsoCodeTwo = "CH", IsoCodeThree = "CHE", PhoneCode = "+41" },
                new Country { Name = "Syrian Arab Republic", IsoCodeTwo = "SY", IsoCodeThree = "SYR", PhoneCode = "+963" },
                new Country { Name = "Tajikistan", IsoCodeTwo = "TJ", IsoCodeThree = "TJK", PhoneCode = "+992" },
                new Country { Name = "Tanzania", IsoCodeTwo = "TZ", IsoCodeThree = "TZA", PhoneCode = "+255" },
                new Country { Name = "Thailand", IsoCodeTwo = "TH", IsoCodeThree = "THA", PhoneCode = "+66" },
                new Country { Name = "Timor-Leste", IsoCodeTwo = "TL", IsoCodeThree = "TLS", PhoneCode = "+670" },
                new Country { Name = "Togo", IsoCodeTwo = "TG", IsoCodeThree = "TGO", PhoneCode = "+228" },
                new Country { Name = "Tonga", IsoCodeTwo = "TO", IsoCodeThree = "TON", PhoneCode = "+676" },
                new Country { Name = "Trinidad and Tobago", IsoCodeTwo = "TT", IsoCodeThree = "TTO", PhoneCode = "+1-868" },
                new Country { Name = "Tunisia", IsoCodeTwo = "TN", IsoCodeThree = "TUN", PhoneCode = "+216" },
                new Country { Name = "Turkey", IsoCodeTwo = "TR", IsoCodeThree = "TUR", PhoneCode = "+90" },
                new Country { Name = "Turkmenistan", IsoCodeTwo = "TM", IsoCodeThree = "TKM", PhoneCode = "+993" },
                new Country { Name = "Tuvalu", IsoCodeTwo = "TV", IsoCodeThree = "TUV", PhoneCode = "+688" },
                new Country { Name = "Uganda", IsoCodeTwo = "UG", IsoCodeThree = "UGA", PhoneCode = "+256" },
                new Country { Name = "Ukraine", IsoCodeTwo = "UA", IsoCodeThree = "UKR", PhoneCode = "+380" },
                new Country { Name = "United Arab Emirates", IsoCodeTwo = "AE", IsoCodeThree = "ARE", PhoneCode = "+971" },
                new Country { Name = "United Kingdom", IsoCodeTwo = "GB", IsoCodeThree = "GBR", PhoneCode = "+44" },
                new Country { Name = "United States of America", IsoCodeTwo = "US", IsoCodeThree = "USA", PhoneCode = "+1" },
                new Country { Name = "Uruguay", IsoCodeTwo = "UY", IsoCodeThree = "URY", PhoneCode = "+598" },
                new Country { Name = "Uzbekistan", IsoCodeTwo = "UZ", IsoCodeThree = "UZB", PhoneCode = "+998" },
                new Country { Name = "Vanuatu", IsoCodeTwo = "VU", IsoCodeThree = "VUT", PhoneCode = "+678" },
                new Country { Name = "Venezuela", IsoCodeTwo = "VE", IsoCodeThree = "VEN", PhoneCode = "+58" },
                new Country { Name = "Viet Nam", IsoCodeTwo = "VN", IsoCodeThree = "VNM", PhoneCode = "+84" },
                new Country { Name = "Yemen", IsoCodeTwo = "YE", IsoCodeThree = "YEM", PhoneCode = "+967" },
                new Country { Name = "Zambia", IsoCodeTwo = "ZM", IsoCodeThree = "ZMB", PhoneCode = "+260" },
                new Country { Name = "Zimbabwe", IsoCodeTwo = "ZW", IsoCodeThree = "ZWE", PhoneCode = "+263" }
            };

            return countries;
        }

        private IEnumerable<Email> CreateEmails()
        {
            emails = new List<Email>()
            {
                new Email { Address = "georgi@mail.com", Type = EmailType.Personal, IsMain = true, Client = clients[0] },
                new Email { Address = "kamen@mail.com", Type = EmailType.Personal, IsMain = true, Client = clients[1] },
                new Email { Address = "hristian@mail.com", Type = EmailType.Personal, IsMain = true, Client = clients[2] },
                new Email { Address = "teodor@mail.com", Type = EmailType.Personal, IsMain = true, Client = clients[3] },
                new Email { Address = "vladislav@mail.com", Type = EmailType.Personal, IsMain = true, Client = clients[4] }
            };

            return emails;
        }

        private IEnumerable<Fine> CreateFines()
        {
            fines = new List<Fine>()
            {
                new Fine { Amount = 12m, Code = "FI-67212345", Reason = FineReason.ReturnDelay, Status = FineStatus.Unpaid, IssueDate = DateTime.Now.AddDays(-2), Checkout = checkouts[8] }
            };

            return fines;
        }

        private IEnumerable<GenreBook> CreateGenreBooks()
        {
            genreBooks = new List<GenreBook>()
            {
                new GenreBook { Genre = genres[3], Book = books[0] },
                new GenreBook { Genre = genres[3], Book = books[1] },
                new GenreBook {Genre = genres[3], Book = books[2]},
                new GenreBook {Genre = genres[3], Book = books[3]},
                new GenreBook {Genre = genres[3], Book = books[4]},
                new GenreBook {Genre = genres[3], Book = books[5]},
                new GenreBook {Genre = genres[3], Book = books[6]},
                new GenreBook {Genre = genres[3], Book = books[7]},
                new GenreBook {Genre = genres[3], Book = books[8]},
                new GenreBook {Genre = genres[3], Book = books[9]},
                new GenreBook {Genre = genres[3], Book = books[10]}
            };

            return genreBooks;
        }

        private IEnumerable<Genre> CreateGenres()
        {
            genres = new List<Genre>()
            {
                new Genre { Name = "Mystery", Description = "Description" },
                new Genre { Name = "Thriller", Description = "Description" },
                new Genre { Name = "Science Fiction", Description = "Description" },
                new Genre { Name = "Fantasy", Description = "Description" },
                new Genre { Name = "Romance", Description = "Description" },
                new Genre { Name = "Historical Fiction", Description = "Description" },
                new Genre { Name = "Horror", Description = "Description" },
                new Genre { Name = "Young Adult", Description = "Description" },
                new Genre { Name = "Dystopian", Description = "Description" },
                new Genre { Name = "Contemporary", Description = "Description" },
                new Genre { Name = "Adventure", Description = "Description" },
                new Genre { Name = "Paranormal", Description = "Description" },
                new Genre { Name = "Graphic Novels", Description = "Description" },
                new Genre { Name = "Memoir", Description = "Description" },
                new Genre { Name = "Biography", Description = "Description" },
                new Genre { Name = "Self-help", Description = "Description" },
                new Genre { Name = "True Crime", Description = "Description" },
                new Genre { Name = "Classic Literature", Description = "Description" },
                new Genre { Name = "Women's Fiction", Description = "Description" },
                new Genre { Name = "Chick Lit", Description = "Description" },
                new Genre { Name = "Magical Realism", Description = "Description" },
                new Genre { Name = "Literary Fiction", Description = "Description" },
                new Genre { Name = "Gnome Fiction", Description = "Description" },
                new Genre { Name = "New Adult", Description = "Description" },
                new Genre { Name = "Steampunk", Description = "Description" },
                new Genre { Name = "Urban Fantasy", Description = "Description" },
                new Genre { Name = "Cozy Mystery", Description = "Description" },
                new Genre { Name = "Psychological Thriller", Description = "Description" },
                new Genre { Name = "Legal Thriller", Description = "Description" },
                new Genre { Name = "Political Thriller", Description = "Description" },
                new Genre { Name = "Military Fiction", Description = "Description" },
                new Genre { Name = "Espionage", Description = "Description" },
                new Genre { Name = "Space Opera", Description = "Description" },
                new Genre { Name = "Cyberpunk", Description = "Description" },
                new Genre { Name = "Apocalyptic", Description = "Description" },
                new Genre { Name = "Post-apocalyptic", Description = "Description" },
                new Genre { Name = "Alternate History", Description = "Description" },
                new Genre { Name = "Superhero Fiction", Description = "Description" },
                new Genre { Name = "Western", Description = "Description" },
                new Genre { Name = "Gothic", Description = "Description" },
                new Genre { Name = "Fairy Tales", Description = "Description" },
                new Genre { Name = "Anthologies", Description = "Description" },
                new Genre { Name = "Short Stories", Description = "Description" },
                new Genre { Name = "Satire", Description = "Description" },
                new Genre { Name = "Humor", Description = "Description" },
                new Genre { Name = "Christian Fiction", Description = "Description" },
                new Genre { Name = "Inspirational", Description = "Description" },
                new Genre { Name = "Coming of Age", Description = "Description" },
                new Genre { Name = "Sports Fiction", Description = "Description" },
                new Genre { Name = "Travel Literature", Description = "Description" }
            };

            return genres;
        }

        private IEnumerable<Language> CreateLanguages()
        {
            languages = new List<Language>
            {
                new Language { Code = "eng", Name = "English" },
                new Language { Code = "spa", Name = "Spanish" },
                new Language { Code = "cmn", Name = "Mandarin Chinese" },
                new Language { Code = "hin", Name = "Hindi" },
                new Language { Code = "ara", Name = "Arabic" },
                new Language { Code = "ben", Name = "Bengali" },
                new Language { Code = "por", Name = "Portuguese" },
                new Language { Code = "rus", Name = "Russian" },
                new Language { Code = "jpn", Name = "Japanese" },
                new Language { Code = "deu", Name = "German" },
                new Language { Code = "bul", Name = "Bulgarian" }
            };

            return languages;
        }

        private IEnumerable<PhoneNumber> CreatePhoneNumbers()
        {
            phoneNumbers = new List<PhoneNumber>()
            {
                new PhoneNumber { Number = "895413322", Type = PhoneNumberType.Mobile, IsMain = true, Client = clients[0], Country = countries[25] },
                new PhoneNumber { Number = "895423327", Type = PhoneNumberType.Mobile, IsMain = true, Client = clients[1], Country = countries[25] },
                new PhoneNumber { Number = "895444325", Type = PhoneNumberType.Mobile, IsMain = true, Client = clients[2], Country = countries[25] },
                new PhoneNumber { Number = "895442326", Type = PhoneNumberType.Mobile, IsMain = true, Client = clients[3], Country = countries[25] },
                new PhoneNumber { Number = "895473321", Type = PhoneNumberType.Mobile, IsMain = true, Client = clients[4], Country = countries[25] }
            };

            return phoneNumbers;
        }

        private IEnumerable<SeriesBook> CreateSeriesBooks()
        {
            seriesBooks = new List<SeriesBook>()
            {
                new SeriesBook { Series = series[1], Book = books[0] },
                new SeriesBook { Series = series[1], Book = books[1] },
                new SeriesBook { Series = series[1], Book = books[2] },
                new SeriesBook { Series = series[2], Book = books[3] },
                new SeriesBook { Series = series[2], Book = books[4] },
                new SeriesBook { Series = series[2], Book = books[5] },
                new SeriesBook { Series = series[2], Book = books[6] },
                new SeriesBook { Series = series[2], Book = books[7] },
                new SeriesBook { Series = series[0], Book = books[8] },
                new SeriesBook { Series = series[0], Book = books[9] },
                new SeriesBook { Series = series[0], Book = books[10] }
            };

            return seriesBooks;
        }

        private IEnumerable<Series> CreateSeries()
        {
            series = new List<Series>()
            {
                new Series { Title = "The Lord of the Rings", Description = "Description" },
                new Series { Title = "Harry Potter", Description = "Description" },
                new Series { Title = "A Song of Ice and Fire", Description = "Description" }
            };

            return series;
        }
        #endregion

    }
}
