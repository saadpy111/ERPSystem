using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Persistence.Configurations
{
    /// <summary>
    /// EF Core configuration for TenantWebsite entity.
    /// Uses JSON column for SiteConfig.
    /// </summary>
    public class TenantWebsiteConfiguration : IEntityTypeConfiguration<TenantWebsite>
    {
        public void Configure(EntityTypeBuilder<TenantWebsite> builder)
        {
            builder.ToTable("TenantWebsites", "Website");

            builder.HasKey(tw => tw.Id);

            builder.Property(tw => tw.TenantId)
                .IsRequired()
                .HasMaxLength(100);

            // Unique constraint: One website per tenant
            builder.HasIndex(tw => tw.TenantId)
                .IsUnique();

            builder.Property(tw => tw.Mode)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(tw => tw.ThemeId);

            builder.Property(tw => tw.IsPublished)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(tw => tw.CreatedAt)
                .IsRequired();

            builder.Property(tw => tw.UpdatedAt)
                .IsRequired();

            // Store SiteConfig as JSON
            builder.OwnsOne(tw => tw.Config, config =>
            {
                config.ToJson();
                
                // Business data
                config.Property(c => c.SiteName).HasMaxLength(200);
                config.Property(c => c.Domain).HasMaxLength(200);
                config.Property(c => c.BusinessType).HasMaxLength(100);
                config.Property(c => c.LogoUrl).HasMaxLength(500);
                
                // Presentation data
                config.OwnsOne(c => c.Colors);
                config.OwnsOne(c => c.Hero);
                config.OwnsMany(c => c.Sections);
            });
        }
    }
}
