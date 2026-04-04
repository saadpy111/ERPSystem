using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class PartnerConfiguration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.ToTable("Partners");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Code).HasMaxLength(50).IsRequired();
            builder.Property(e => e.NameAr).HasMaxLength(200).IsRequired();
            builder.Property(e => e.NameEn).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Phone).HasMaxLength(50);
            builder.Property(e => e.Email).HasMaxLength(200);
            builder.Property(e => e.TaxNumber).HasMaxLength(100);
            builder.Property(e => e.CreditLimit).HasColumnType("decimal(18,6)");

            builder.HasOne(e => e.DefaultCurrency)
                .WithMany()
                .HasForeignKey(e => e.DefaultCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.DefaultTax)
                .WithMany(e => e.Partners)
                .HasForeignKey(e => e.DefaultTaxId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => new { e.TenantId, e.Code }).IsUnique();
            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
            builder.HasIndex(e => e.DefaultCurrencyId);
            builder.HasIndex(e => e.DefaultTaxId);
        }
    }
}
