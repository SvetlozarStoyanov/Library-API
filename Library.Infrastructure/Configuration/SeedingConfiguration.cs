using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class SeedingConfiguration : IEntityTypeConfiguration<Seeding>
    {
        public void Configure(EntityTypeBuilder<Seeding> builder)
        {
            builder.HasKey(s => s.Id);
        }
    }
}
