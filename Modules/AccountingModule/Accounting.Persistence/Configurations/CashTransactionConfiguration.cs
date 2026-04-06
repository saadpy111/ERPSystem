using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class CashTransactionConfiguration : IEntityTypeConfiguration<CashTransaction>
    {
        public void Configure(EntityTypeBuilder<CashTransaction> builder)
        {
            builder.ToTable("CashTransactions");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.Amount).HasColumnType("decimal(18,6)");
            builder.Property(e => e.BaseAmount).HasColumnType("decimal(18,6)");
            builder.Property(e => e.ExchangeRate).HasColumnType("decimal(18,6)");

            builder.HasOne(e => e.CashAccount)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.CashAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.OffsetAccount)
                .WithMany()
                .HasForeignKey(e => e.OffsetAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Partner)
                .WithMany()
                .HasForeignKey(e => e.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Currency)
                .WithMany()
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.CashAccountId);
            builder.HasIndex(e => e.OffsetAccountId);
            builder.HasIndex(e => e.PartnerId);
            builder.HasIndex(e => e.CurrencyId);
            builder.HasIndex(e => e.TenantId);
        }
    }
}
