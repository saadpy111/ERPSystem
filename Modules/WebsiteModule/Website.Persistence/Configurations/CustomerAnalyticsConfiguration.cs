using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class CustomerAnalyticsConfiguration : IEntityTypeConfiguration<CustomerAnalytics>
    {
        public void Configure(EntityTypeBuilder<CustomerAnalytics> builder)
        {
            builder.ToTable("CustomerAnalytics");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(x => x.TotalSpent)
                .HasPrecision(18, 2);

            builder.Property(x => x.AverageOrderValue)
                .HasPrecision(18, 2);

            builder.Property(x => x.FavoritePurchaseDay)
                .HasMaxLength(20);

            builder.Property(x => x.MostPurchasedCategory)
                .HasMaxLength(200);

            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Unique index on (TenantId, UserId)
            builder.HasIndex(x => new { x.TenantId, x.UserId })
                .IsUnique();
        }
    }
}
