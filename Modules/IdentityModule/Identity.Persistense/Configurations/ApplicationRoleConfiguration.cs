using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistense.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.Property(r => r.TenantId)
                .IsRequired();

            builder.Property(r => r.Scope)
                .IsRequired();

            builder.HasOne(r => r.Tenant)
                .WithMany(t => t.Roles)
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.UserRoles)
                    .WithOne(t => t.Role)
                    .HasForeignKey(r => r.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => new { r.Name, r.TenantId, r.Scope })
                .IsUnique();
        }
    }
}
