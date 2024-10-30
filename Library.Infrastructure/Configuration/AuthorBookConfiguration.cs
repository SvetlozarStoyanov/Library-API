using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class AuthorBookConfiguration : IEntityTypeConfiguration<AuthorBook>
    {
        public void Configure(EntityTypeBuilder<AuthorBook> entityBuilder)
        {
            entityBuilder.HasKey(ab => ab.Id);

            entityBuilder.HasQueryFilter(ab => ab.Book.Status == BookStatus.Active);
        }
    }
}
