using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class SubscriptionHistoryConfiguration : IEntityTypeConfiguration<SubscriptionHistory>
    {
        public void Configure(EntityTypeBuilder<SubscriptionHistory> builder)
        {
            builder.ToTable("SubscriptionHistory", "Subscription");

            builder.HasKey(sh => sh.Id);

            builder.Property(sh => sh.FromPlanId)
                .HasMaxLength(50);

            builder.Property(sh => sh.ToPlanId)
                .HasMaxLength(50);

            builder.Property(sh => sh.FromStatus)
                .HasMaxLength(20);

            builder.Property(sh => sh.ToStatus)
                .HasMaxLength(20);

            builder.Property(sh => sh.Notes)
                .HasMaxLength(500);

            builder.Property(sh => sh.PerformedBy)
                .IsRequired()
                .HasMaxLength(100);

            // Indexes
            builder.HasIndex(sh => sh.TenantSubscriptionId);
            builder.HasIndex(sh => sh.CreatedAt).IsDescending();
            builder.HasIndex(sh => sh.EventType);

            // Relationship
            builder.HasOne(sh => sh.Subscription)
                .WithMany(ts => ts.History)
                .HasForeignKey(sh => sh.TenantSubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
