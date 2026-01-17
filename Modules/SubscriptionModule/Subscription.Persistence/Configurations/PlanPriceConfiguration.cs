using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class PlanPriceConfiguration : IEntityTypeConfiguration<PlanPrice>
    {
        public void Configure(EntityTypeBuilder<PlanPrice> builder)
        {
            builder.ToTable("PlanPrices", "Subscription");

            builder.HasKey(pp => pp.Id);

            builder.Property(pp => pp.CurrencyCode)
                .IsRequired()
                .HasMaxLength(3); // ISO 4217

            builder.Property(pp => pp.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Unique constraint: Plan + Currency + Interval
            builder.HasIndex(pp => new { pp.PlanId, pp.CurrencyCode, pp.Interval })
                .IsUnique();

            builder.HasIndex(pp => pp.IsActive);

            // Relationship
            builder.HasOne(pp => pp.Plan)
                .WithMany(p => p.Prices)
                .HasForeignKey(pp => pp.PlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
