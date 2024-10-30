using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> entitybuilder)
        {
            entitybuilder.HasKey(b => b.Id);

            entitybuilder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(100);

            entitybuilder.Property(b => b.ISBN)
                .HasMaxLength(13);

            entitybuilder.Property(b => b.Description)
                .HasMaxLength(350)
                .IsRequired();

            entitybuilder.Property(b => b.Status)
                .HasConversion<string>()
                .IsRequired();

            entitybuilder.HasOne(b => b.Language)
                .WithMany(l => l.Books)
                .HasForeignKey(b => b.LanguageId)
                .IsRequired();

            entitybuilder.HasOne(b => b.Country)
                .WithMany(l => l.Books)
                .HasForeignKey(b => b.CountryId)
                .IsRequired();

            entitybuilder
                .HasMany(b => b.Acquisitions)
                .WithOne(bd => bd.Book)
                .IsRequired();

            entitybuilder
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity<AuthorBook>();

            entitybuilder
                .HasMany(b => b.Genres)
                .WithMany(g => g.Books)
                .UsingEntity<GenreBook>();

            entitybuilder
                .HasMany(b => b.Series)
                .WithMany(s => s.Books)
                .UsingEntity<SeriesBook>();

            entitybuilder
                .HasMany(b => b.Checkouts)
                .WithOne(c => c.Book)
                .HasForeignKey(c => c.BookId)
                .IsRequired();

            entitybuilder
                .HasIndex(b => b.ISBN)
                .IsUnique();

            entitybuilder.HasQueryFilter(b => b.Status == BookStatus.Active);
        }
    }
}
