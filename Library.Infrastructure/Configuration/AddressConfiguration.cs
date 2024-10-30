using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entityBuilder)
        {
            entityBuilder.HasKey(a => a.Id);

            entityBuilder
                .HasOne(a => a.Client)
                .WithMany(a => a.Addresses)
                .HasForeignKey(a => a.ClientId);

            entityBuilder.Property(a => a.City)
                .HasMaxLength(170)
                .IsRequired();

            entityBuilder.Property(a => a.AddressLine)
                .HasMaxLength(170)
                .IsRequired();

            entityBuilder.Property(a => a.PostalCode)
                .HasMaxLength(12)
                .IsRequired();

            entityBuilder.Property(a => a.Type)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder.Property(a => a.Status)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder
                .HasOne(a => a.Country)
                .WithMany(a => a.Addresses)
                .HasForeignKey(a => a.CountryId);

            //entityBuilder
            //    .HasIndex(a => new { a.ClientId, a.Type, a.Status })
            //    .IsUnique()
            //    .HasFilter($"[Type] = '{AddressType.Residency}' AND [Status] = '{AddressStatus.Active}'");


            entityBuilder.HasQueryFilter(a => a.Status == AddressStatus.Active);
        }
    }
}
