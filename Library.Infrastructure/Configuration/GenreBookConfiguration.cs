using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class GenreBookConfiguration : IEntityTypeConfiguration<GenreBook>
    {
        public void Configure(EntityTypeBuilder<GenreBook> entityBuilder)
        {
            entityBuilder.HasKey(gb => gb.Id);

            entityBuilder.HasQueryFilter(gb => gb.Book.Status == BookStatus.Active);
        }
    }
}
