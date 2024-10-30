using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class BookAcquisitionConfiguration : IEntityTypeConfiguration<BookAcquisition>
    {
        public void Configure(EntityTypeBuilder<BookAcquisition> entityBuilder)
        {
            entityBuilder.HasKey(bd => bd.Id);

            entityBuilder.Property(bd => bd.Type)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder.HasOne(bd => bd.Book)
                .WithMany(b => b.Acquisitions)
                .IsRequired();

            entityBuilder
                .HasIndex(bd => new { bd.BookId, bd.Type })
                .IsUnique()
                .HasFilter($"[Type] = '{BookAcquisitionType.Initial}'");

            entityBuilder.HasQueryFilter(bd => bd.Book.Status == BookStatus.Active);
        }
    }
}
