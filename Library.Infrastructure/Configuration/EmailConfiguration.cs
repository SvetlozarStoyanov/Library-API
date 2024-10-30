using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class EmailConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> entityBuilder)
        {
            entityBuilder.HasKey(e => e.Id);

            entityBuilder.Property(e => e.Address)
                .IsRequired();

            entityBuilder.Property(e => e.Type)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder.Property(e => e.Status)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder.HasOne(e => e.Client)
                .WithMany(c => c.Emails)
                .HasForeignKey(e => e.ClientId)
                .IsRequired();

            entityBuilder
                .HasIndex(e => e.Address)
                .IsUnique();

            //entityBuilder
            //    .HasIndex(e => new { e.ClientId, e.IsMain, e.Status })
            //    .IsUnique()
            //    .HasFilter($"[IsMain] = 1 AND [Status] = '{EmailStatus.Active}'");

            entityBuilder.HasQueryFilter(e => e.Status == EmailStatus.Active);
        }
    }
}
