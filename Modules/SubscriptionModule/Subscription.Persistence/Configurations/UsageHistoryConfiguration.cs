using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class UsageHistoryConfiguration : IEntityTypeConfiguration<UsageHistory>
    {
        public void Configure(EntityTypeBuilder<UsageHistory> builder)
        {
            builder.ToTable("UsageHistory", "Subscription");

            builder.HasKey(uh => uh.Id);

            builder.Property(uh => uh.OverageDetails)
                .HasMaxLength(1000); // JSON

            // Indexes
            builder.HasIndex(uh => uh.TenantSubscriptionId);
            builder.HasIndex(uh => uh.PeriodEnd).IsDescending();
            builder.HasIndex(uh => uh.HasOverage);

            // Relationship
            builder.HasOne(uh => uh.Subscription)
                .WithMany(ts => ts.UsageHistory)
                .HasForeignKey(uh => uh.TenantSubscriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
