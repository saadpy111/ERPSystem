using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistense.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants", "Identity");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(t => t.Code)
                .IsUnique();

            builder.Property(t => t.IsActive)
                .IsRequired();

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            // Note: Website configuration moved to WebsiteModule
            // Note: Permission is now global and has no relationship with Tenant
            // RolePermission and UserPermission maintain tenant isolation through their TenantId
        }
    }
}
