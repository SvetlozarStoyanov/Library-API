using Library.Infrastructure.Entities;
using Library.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class SeriesBookConfiguration : IEntityTypeConfiguration<SeriesBook>
    {
        public void Configure(EntityTypeBuilder<SeriesBook> entityBuilder)
        {
            entityBuilder.HasKey(sb => sb.Id);

            entityBuilder.HasQueryFilter(sb => sb.Book.Status == BookStatus.Active);
        }
    }
}
