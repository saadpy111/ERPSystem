using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currencies");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Code).HasMaxLength(3).IsRequired();
            builder.Property(e => e.NameAr).HasMaxLength(100).IsRequired();
            builder.Property(e => e.NameEn).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Symbol).HasMaxLength(10).IsRequired();

            // Indexes
            builder.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
        }
    }
}
