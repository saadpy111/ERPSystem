using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class VoucherLineConfiguration : IEntityTypeConfiguration<VoucherLine>
    {
        public void Configure(EntityTypeBuilder<VoucherLine> builder)
        {
            builder.ToTable("VoucherLines");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Debit).HasColumnType("decimal(18,6)");
            builder.Property(e => e.Credit).HasColumnType("decimal(18,6)");

            builder.HasOne(e => e.Voucher)
                .WithMany(e => e.Lines)
                .HasForeignKey(e => e.VoucherId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CostCenter)
                .WithMany()
                .HasForeignKey(e => e.CostCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
            builder.HasIndex(e => e.VoucherId);
            builder.HasIndex(e => e.AccountId);
        }
    }
}
