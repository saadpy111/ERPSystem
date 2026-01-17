using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class TenantSubscriptionConfiguration : IEntityTypeConfiguration<TenantSubscription>
    {
        public void Configure(EntityTypeBuilder<TenantSubscription> builder)
        {
            builder.ToTable("TenantSubscriptions", "Subscription");

            builder.HasKey(ts => ts.Id);

            builder.Property(ts => ts.BillingCycle)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(ts => ts.CurrencyCode)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(ts => ts.ExternalSubscriptionId)
                .HasMaxLength(100);

            builder.Property(ts => ts.ExternalCustomerId)
                .HasMaxLength(100);

            builder.Property(ts => ts.CancelReason)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(ts => ts.TenantId).IsUnique(); // One subscription per tenant
            builder.HasIndex(ts => ts.Status);
            builder.HasIndex(ts => ts.CurrentPeriodEnd);
            builder.HasIndex(ts => ts.BillingAnchorDay); // For quota reset job

            // Relationships
            // Note: No navigation to Tenant (cross-module boundary)
            builder.HasOne(ts => ts.Plan)
                .WithMany(p => p.TenantSubscriptions)
                .HasForeignKey(ts => ts.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ts => ts.History)
                .WithOne(h => h.Subscription)
                .HasForeignKey(h => h.TenantSubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ts => ts.UsageHistory)
                .WithOne(uh => uh.Subscription)
                .HasForeignKey(uh => uh.TenantSubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
