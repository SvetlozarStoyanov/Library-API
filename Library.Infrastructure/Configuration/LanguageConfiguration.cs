using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder
                .Property(x => x.Code)
                .HasMaxLength(3)
                .IsRequired();

            entityBuilder.HasMany(l => l.Books)
                .WithOne(b => b.Language)
                .HasForeignKey(b => b.LanguageId)
                .IsRequired();

            entityBuilder
                .HasIndex(l => l.Code)
                .IsUnique();
        }
    }
}
