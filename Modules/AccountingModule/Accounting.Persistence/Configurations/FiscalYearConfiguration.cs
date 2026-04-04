using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class FiscalYearConfiguration : IEntityTypeConfiguration<FiscalYear>
    {
        public void Configure(EntityTypeBuilder<FiscalYear> builder)
        {
            builder.ToTable("FiscalYears");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
        }
    }
}
