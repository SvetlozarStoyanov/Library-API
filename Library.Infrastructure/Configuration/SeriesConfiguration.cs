using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class SeriesConfiguration : IEntityTypeConfiguration<Series>
    {
        public void Configure(EntityTypeBuilder<Series> entityBuilder)
        {
            entityBuilder.HasKey(s => s.Id);

            entityBuilder
                .Property(s => s.Title)
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder
                .Property(s => s.Description)
                .HasMaxLength(350)
                .IsRequired();

            entityBuilder.HasMany(s => s.Books)
                .WithMany(b => b.Series)
                .UsingEntity<SeriesBook>();

            entityBuilder
                .HasIndex(s => s.Title)
                .IsUnique();
        }
    }
}
