using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("Vouchers");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.VoucherNumber).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Amount).HasColumnType("decimal(18,6)");

            builder.HasOne(e => e.Partner)
                .WithMany(e => e.Vouchers)
                .HasForeignKey(e => e.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JournalEntry)
                .WithMany(e => e.Vouchers)
                .HasForeignKey(e => e.JournalEntryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(e => new { e.TenantId, e.VoucherNumber }).IsUnique();
            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => new { e.TenantId, e.Id });
            builder.HasIndex(e => e.PartnerId);
            builder.HasIndex(e => e.CurrencyId);
            builder.HasIndex(e => e.JournalEntryId);
        }
    }
}
