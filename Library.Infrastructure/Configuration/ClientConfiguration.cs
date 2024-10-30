using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> entityBuilder)
        {
            entityBuilder.HasKey(c => c.Id);

            entityBuilder.Property(c => c.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder.Property(c => c.MiddleName)
                .HasMaxLength(100);

            entityBuilder.Property(c => c.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder
                .Property(c => c.UnifiedCivilNumber)
                .HasMaxLength(10);

            entityBuilder
                .HasIndex(c => c.Code)
                .IsUnique();
        }
    }
}
