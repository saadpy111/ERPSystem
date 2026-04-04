using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class SequenceConfiguration : IEntityTypeConfiguration<Sequence>
    {
        public void Configure(EntityTypeBuilder<Sequence> builder)
        {
            builder.ToTable("Sequences");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Prefix).HasMaxLength(20).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
        }
    }
}
