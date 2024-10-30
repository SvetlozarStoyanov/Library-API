using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumber>
    {
        public void Configure(EntityTypeBuilder<PhoneNumber> entityBuilder)
        {
            entityBuilder.HasKey(pn => pn.Id);

            entityBuilder
                .Property(pn => pn.Number)
                .HasMaxLength(15)
                .IsRequired();

            entityBuilder
                .Property(pn => pn.Type)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder
                .Property(pn => pn.Status)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder
                .HasOne(pn => pn.Client)
                .WithMany(c => c.PhoneNumbers)
                .HasForeignKey(pn => pn.ClientId)
                .IsRequired();

            entityBuilder
                .HasOne(pn => pn.Country)
                .WithMany(c => c.PhoneNumbers)
                .HasForeignKey(pn => pn.CountryId)
                .IsRequired();

            //entityBuilder
            //    .HasIndex(pn => new { pn.ClientId, pn.IsMain })
            //    .IsUnique()
            //    .HasFilter($"[IsMain] = 1 AND [Status] = '{PhoneNumberStatus.Active}'");

            //entityBuilder
            //    .HasIndex(pn => new { pn.Code, pn.Number })
            //    .IsUnique();

            entityBuilder.HasQueryFilter(pn => pn.Status == PhoneNumberStatus.Active);
        }
    }
}
