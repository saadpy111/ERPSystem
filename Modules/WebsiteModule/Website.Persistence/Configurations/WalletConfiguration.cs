using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets", "Website");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.TenantId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(w => w.CurrentBalance)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasIndex(w => w.TenantId);

            builder.HasMany(w => w.Transactions)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(w => w.WithdrawalRequests)
                .WithOne(r => r.Wallet)
                .HasForeignKey(r => r.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
