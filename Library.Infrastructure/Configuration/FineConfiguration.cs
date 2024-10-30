using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class FineConfiguration : IEntityTypeConfiguration<Fine>
    {
        public void Configure(EntityTypeBuilder<Fine> entityBuilder)
        {
            entityBuilder.HasKey(f => f.Id);

            entityBuilder.Property(f => f.Status)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder.Property(fc => fc.Amount)
                .HasPrecision(18, 2);

            entityBuilder.Property(f => f.Reason)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder.HasOne(f => f.Checkout)
                .WithMany(c => c.Fines)
                .HasForeignKey(f => f.CheckoutId)
                .IsRequired();

            entityBuilder.HasQueryFilter(f => f.Checkout.Book.Status == BookStatus.Active);
        }
    }
}
