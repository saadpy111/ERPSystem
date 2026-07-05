using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments", "Subscription");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.TenantId)
                .HasMaxLength(50);

            builder.Property(p => p.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(p => p.Purpose)
                .IsRequired();

            builder.Property(p => p.TargetId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.ExpectedAmountCents)
                .IsRequired();

            builder.Property(p => p.CurrencyCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.Interval)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.CustomerFirstName)
                .HasMaxLength(100);

            builder.Property(p => p.ClientSecret)
                .HasMaxLength(500);

            builder.Property(p => p.CheckoutUrl)
                .HasMaxLength(1000);

            builder.Property(p => p.CustomerLastName)
                .HasMaxLength(100);

            builder.Property(p => p.CustomerEmail)
                .HasMaxLength(150);

            builder.Property(p => p.CustomerPhone)
                .HasMaxLength(50);

            builder.Property(p => p.Payload)
                .HasColumnType("nvarchar(max)");

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            // Indexes
            builder.HasIndex(p => new { p.TenantId, p.Status });
            builder.HasIndex(p => new { p.TenantId, p.Purpose, p.TargetId, p.Status });
            builder.HasIndex(p => new { p.UserId, p.Purpose, p.TargetId, p.Status });
        }
    }
}
