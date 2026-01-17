using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    /// <summary>
    /// EF Core configuration for Theme entity.
    /// Uses JSON column for ThemeConfig.
    /// </summary>
    public class ThemeConfiguration : IEntityTypeConfiguration<Theme>
    {
        public void Configure(EntityTypeBuilder<Theme> builder)
        {
            builder.ToTable("Themes", "Website");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(t => t.Code)
                .IsUnique();

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.PreviewImage)
                .HasMaxLength(500);

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.UpdatedAt)
                .IsRequired();

            // Store ThemeConfig as JSON
            builder.OwnsOne(t => t.Config, config =>
            {
                config.ToJson();
                
                config.OwnsOne(c => c.Colors);
                config.OwnsOne(c => c.Hero);
                config.OwnsMany(c => c.Sections);
            });
        }
    }
}
