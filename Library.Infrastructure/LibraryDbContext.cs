using Library.Infrastructure.Configuration;
using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAcquisition> BookAcquisitions { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<AuthorBook> AuthorsBooks { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }
        public DbSet<ClientCard> ClientCards { get; set; }
        public DbSet<ClientCardType> ClientCardTypes { get; set; }
        public DbSet<ClientCardStatusChange> ClientCardStatusChanges { get; set; }
        public DbSet<GenreBook> GenresBooks { get; set; }
        public DbSet<SeriesBook> SeriesBooks { get; set; }
        public DbSet<Seeding> Seedings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EntityConfiguration(modelBuilder);


            base.OnModelCreating(modelBuilder);
        }

        private void EntityConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new ClientCardTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new SeriesConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BookAcquisitionConfiguration());
            modelBuilder.ApplyConfiguration(new CheckoutConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new EmailConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneNumberConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorBookConfiguration());
            modelBuilder.ApplyConfiguration(new GenreBookConfiguration());
            modelBuilder.ApplyConfiguration(new SeriesBookConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new ClientCardConfiguration());
            modelBuilder.ApplyConfiguration(new ClientCardStatusChangeConfiguration());
            modelBuilder.ApplyConfiguration(new FineConfiguration());
            modelBuilder.ApplyConfiguration(new SeedingConfiguration());
        }
    }
}
