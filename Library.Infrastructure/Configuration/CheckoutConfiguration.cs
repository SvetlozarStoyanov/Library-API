using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class CheckoutConfiguration : IEntityTypeConfiguration<Checkout>
    {
        public void Configure(EntityTypeBuilder<Checkout> entityBuilder)
        {
            entityBuilder.HasKey(ch => ch.Id);

            entityBuilder
                .Property(ch => ch.Status)
                .HasConversion<string>();

            entityBuilder
                .HasOne(ch => ch.ClientCard)
                .WithMany(cc => cc.Checkouts)
                .HasForeignKey(c => c.ClientCardId)
                .IsRequired();

            entityBuilder
                .HasOne(ch => ch.Book)
                .WithMany(b => b.Checkouts)
                .HasForeignKey(ch => ch.BookId)
                .IsRequired();

            entityBuilder.HasQueryFilter(c => c.Book.Status == BookStatus.Active);
        }
    }
}
