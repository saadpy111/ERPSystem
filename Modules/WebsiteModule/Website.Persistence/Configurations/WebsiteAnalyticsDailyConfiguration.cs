using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class WebsiteAnalyticsDailyConfiguration : IEntityTypeConfiguration<WebsiteAnalyticsDaily>
    {
        public void Configure(EntityTypeBuilder<WebsiteAnalyticsDaily> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenantId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.Revenue)
                .HasPrecision(18, 2);

            builder.HasIndex(x => new { x.TenantId, x.Date }).IsUnique();
        }
    }
}
