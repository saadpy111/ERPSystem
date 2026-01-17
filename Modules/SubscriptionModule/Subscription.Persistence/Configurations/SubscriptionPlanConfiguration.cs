using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.ToTable("SubscriptionPlans", "Subscription");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.DisplayName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            // Indexes
            builder.HasIndex(p => p.Code).IsUnique();
            builder.HasIndex(p => p.Name).IsUnique();
            builder.HasIndex(p => p.IsActive);
            builder.HasIndex(p => p.SortOrder);

            // Relationships
            builder.HasMany(p => p.PlanModules)
                .WithOne(pm => pm.Plan)
                .HasForeignKey(pm => pm.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Prices)
                .WithOne(pp => pp.Plan)
                .HasForeignKey(pp => pp.PlanId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.TenantSubscriptions)
                .WithOne(ts => ts.Plan)
                .HasForeignKey(ts => ts.PlanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
