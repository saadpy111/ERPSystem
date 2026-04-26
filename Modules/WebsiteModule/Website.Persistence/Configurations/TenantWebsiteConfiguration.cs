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
                config.Property(c => c.about_the_site).HasMaxLength(1000);
                config.Property(c => c.location).HasMaxLength(300);
                config.Property(c => c.phone).HasMaxLength(50);
                config.Property(c => c.email).HasMaxLength(200);

                // Presentation data
                config.OwnsOne(c => c.Colors);

                config.OwnsOne(c => c.ContactUsImages, contact =>
                {
                    contact.OwnsOne(x => x.ContactUsImg, img =>
                    {
                        img.OwnsOne(i => i.Style);
                    });

                    contact.OwnsOne(x => x.ClientOImg, img =>
                    {
                        img.OwnsOne(i => i.Style);
                    });
                });
                config.OwnsOne(c => c.Hero, hero =>
                {
                    hero.OwnsOne(h => h.Title, title =>
                    {
                        title.OwnsOne(t => t.Style);
                    });

                    hero.OwnsOne(h => h.Subtitle, subtitle =>
                    {
                        subtitle.OwnsOne(s => s.Style);
                    });

                    hero.OwnsOne(h => h.ButtonText, button =>
                    {
                        button.OwnsOne(b => b.Style);
                    });

                    hero.OwnsOne(h => h.BackgroundImage, image =>
                    {
                        image.OwnsOne(i => i.Style);
                    });
                });
                config.OwnsMany(c => c.Sections, section =>
                {
                    section.OwnsOne(s => s.Title, t =>
                    {
                        t.OwnsOne(x => x.Style);
                    });

                    section.OwnsOne(s => s.Subtitle, t =>
                    {
                        t.OwnsOne(x => x.Style);
                    });

                    section.OwnsOne(s => s.ButtonText, t =>
                    {
                        t.OwnsOne(x => x.Style);
                    });

                    section.OwnsOne(s => s.BackgroundImage, img =>
                    {
                        img.OwnsOne(i => i.Style);
                    });
                });
            });
        }
    }
}
