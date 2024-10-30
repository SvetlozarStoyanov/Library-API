using Library.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configuration
{
    public class ClientCardStatusChangeConfiguration : IEntityTypeConfiguration<ClientCardStatusChange>
    {
        public void Configure(EntityTypeBuilder<ClientCardStatusChange> builder)
        {
            builder.HasKey(ccsc => ccsc.Id);

            builder.Property(ccsc => ccsc.Reason)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(ccsc => ccsc.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(ccsc => ccsc.ClientCard)
                .WithMany(cc => cc.StatusChanges)
                .HasForeignKey(ccsc => ccsc.ClientCardId)
                .IsRequired();
        }
    }
}
