using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.ToTable("PaymentTransactions", "Subscription");

            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.PaymentId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(pt => pt.GatewayTransactionId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pt => pt.PaymentMethod)
                .HasMaxLength(50);

            builder.Property(pt => pt.GatewayStatus)
                .HasMaxLength(100);

            builder.Property(pt => pt.ErrorCode)
                .HasMaxLength(100);

            builder.Property(pt => pt.ErrorMessage)
                .HasMaxLength(500);

            builder.Property(pt => pt.Notes)
                .HasMaxLength(1000);

            builder.Property(pt => pt.WebhookReceivedAt)
                .IsRequired();

            builder.Property(pt => pt.CreatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(pt => pt.Payment)
                .WithMany(p => p.Transactions)
                .HasForeignKey(pt => pt.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(pt => pt.GatewayTransactionId);
            builder.HasIndex(pt => new { pt.PaymentId, pt.GatewayTransactionId });
        }
    }
}
