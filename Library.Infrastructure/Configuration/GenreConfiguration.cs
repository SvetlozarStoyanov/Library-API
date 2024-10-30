using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> entityBuilder)
        {
            entityBuilder.HasKey(g => g.Id);

            entityBuilder.Property(g => g.Name)
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder
                .Property(s => s.Description)
                .HasMaxLength(350)
                .IsRequired();

            entityBuilder
                .HasMany(g => g.Books)
                .WithMany(b => b.Genres)
                .UsingEntity<GenreBook>();

            entityBuilder
                .HasIndex(g => g.Name)
                .IsUnique();
        }
    }
}
