using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class ClientCardConfiguration : IEntityTypeConfiguration<ClientCard>
    {
        public void Configure(EntityTypeBuilder<ClientCard> entityBuilder)
        {
            entityBuilder.HasKey(cc => cc.Id);

            entityBuilder.Property(cc => cc.Status)
                .HasConversion<string>()
                .IsRequired();

            entityBuilder.HasOne(cc => cc.Client)
                .WithMany(c => c.ClientCards)
                .HasForeignKey(c => c.ClientId)
                .IsRequired();

            entityBuilder.HasMany(cc => cc.Checkouts)
                .WithOne(c => c.ClientCard)
                .HasForeignKey(c => c.ClientCardId)
                .IsRequired();

            entityBuilder.HasMany(cc => cc.StatusChanges)
                .WithOne(cca => cca.ClientCard)
                .HasForeignKey(cca => cca.ClientCardId)
                .IsRequired();

        }
    }
}
