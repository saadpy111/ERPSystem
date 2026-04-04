using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class TaxConfiguration : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.ToTable("Taxes");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Rate).HasColumnType("decimal(18,6)");

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
        }
    }
}
