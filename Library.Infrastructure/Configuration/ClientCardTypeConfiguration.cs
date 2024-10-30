using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class ClientCardTypeConfiguration : IEntityTypeConfiguration<ClientCardType>
    {
        public void Configure(EntityTypeBuilder<ClientCardType> builder)
        {
            builder.HasKey(cct => cct.Id);

            builder
                .Property(cct => cct.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasMany(cct => cct.ClientCards)
                .WithOne(cc => cc.ClientCardType)
                .HasForeignKey(cc => cc.ClientCardTypeId)
                .IsRequired();

            builder.HasIndex(cct => cct.Name)
                .IsUnique();
        }
    }
}
