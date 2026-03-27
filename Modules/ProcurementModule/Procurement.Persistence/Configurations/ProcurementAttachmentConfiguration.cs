using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Persistence.Configurations
{
    public class ProcurementAttachmentConfiguration : IEntityTypeConfiguration<ProcurementAttachment>
    {
        public void Configure(EntityTypeBuilder<ProcurementAttachment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName).HasMaxLength(200);
            builder.Property(x => x.FileUrl).HasMaxLength(200);
            builder.Property(x => x.EntityType).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(250);

            // Multi-tenancy indexes
            builder.HasIndex(x => x.TenantId);
            builder.HasIndex(x => new { x.TenantId, x.Id });
        }
    }
}
