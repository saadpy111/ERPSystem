using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class WithdrawalRequestConfiguration : IEntityTypeConfiguration<WithdrawalRequest>
    {
        public void Configure(EntityTypeBuilder<WithdrawalRequest> builder)
        {
            builder.ToTable("WithdrawalRequests", "Website");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.TenantId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.WalletId)
                .IsRequired();

            builder.Property(r => r.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(r => r.RequestedAt)
                .IsRequired();

            builder.Property(r => r.ReviewedBy)
                .HasMaxLength(450);

            builder.Property(r => r.Notes)
                .HasMaxLength(500);

            builder.HasIndex(r => r.TenantId);
            builder.HasIndex(r => r.Status);
            builder.HasIndex(r => new { r.WalletId, r.Status });
        }
    }
}
