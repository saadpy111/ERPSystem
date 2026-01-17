using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistense.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions", "Identity");

            builder.HasKey(rp => rp.Id);

            builder.Property(rp => rp.RoleId)
                .IsRequired();

            builder.Property(rp => rp.PermissionId)
                .IsRequired();

            builder.Property(rp => rp.TenantId)
                .IsRequired();

            builder.Property(rp => rp.AssignedAt)
                .IsRequired();

            // Unique constraint: A role can have the same permission only once per tenant
            builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId, rp.TenantId })
                .IsUnique();

            // Relationships
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
