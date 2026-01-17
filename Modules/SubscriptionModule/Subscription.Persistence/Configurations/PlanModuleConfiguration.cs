using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class PlanModuleConfiguration : IEntityTypeConfiguration<PlanModule>
    {
        public void Configure(EntityTypeBuilder<PlanModule> builder)
        {
            builder.ToTable("PlanModules", "Subscription");

            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.ModuleName)
                .IsRequired()
                .HasMaxLength(50);

            // Unique constraint: Plan + Module
            builder.HasIndex(pm => new { pm.PlanId, pm.ModuleName })
                .IsUnique();

            // Relationship
            builder.HasOne(pm => pm.Plan)
                .WithMany(p => p.PlanModules)
                .HasForeignKey(pm => pm.PlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
