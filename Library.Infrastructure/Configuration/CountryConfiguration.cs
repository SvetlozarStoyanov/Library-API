using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.IsoCodeTwo)
                .HasMaxLength(2)
                .HasColumnName("ISO_2");

            builder.Property(c => c.IsoCodeThree)
                .HasMaxLength(3)
                .HasColumnName("ISO_3");

            builder.HasMany(b => b.Addresses)
                .WithOne(a => a.Country)
                .HasForeignKey(c => c.CountryId)
                .IsRequired();

            builder.HasMany(b => b.Books)
                .WithOne(a => a.Country)
                .HasForeignKey(c => c.CountryId)
                .IsRequired();

            builder
                .HasIndex(c => c.Name)
                .IsUnique();

            builder
                .HasIndex(c => c.IsoCodeTwo)
                .IsUnique();

            builder
                .HasIndex(c => c.IsoCodeThree)
                .IsUnique();
        }
    }
}
