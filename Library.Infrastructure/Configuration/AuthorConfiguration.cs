using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> entityBuilder)
        {
            entityBuilder.HasKey(a => a.Id);

            entityBuilder.Property(a => a.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder.Property(a => a.MiddleName)
                .HasMaxLength(100);

            entityBuilder.Property(a => a.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder.Property(a => a.Description)
                .HasMaxLength(350)
                .IsRequired();

            entityBuilder.HasMany(a => a.Books)
                .WithMany(b => b.Authors)
                .UsingEntity<AuthorBook>();
        }
    }
}
