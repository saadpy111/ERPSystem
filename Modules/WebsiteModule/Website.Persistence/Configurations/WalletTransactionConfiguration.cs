using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("WalletTransactions", "Website");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.TenantId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.WalletId)
                .IsRequired();

            builder.Property(t => t.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(t => t.BalanceBefore)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(t => t.BalanceAfter)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(t => t.Reference)
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.HasIndex(t => t.TenantId);
            builder.HasIndex(t => new { t.WalletId, t.CreatedAt });
        }
    }
}
