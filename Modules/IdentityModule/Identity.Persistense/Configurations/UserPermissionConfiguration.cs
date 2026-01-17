using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistense.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.ToTable("UserPermissions", "Identity");

            builder.HasKey(up => up.Id);

            builder.Property(up => up.UserId)
                .IsRequired();

            builder.Property(up => up.PermissionId)
                .IsRequired();

            builder.Property(up => up.TenantId)
                .IsRequired();

            builder.Property(up => up.AssignedAt)
                .IsRequired();

            // Unique constraint: A user can have the same permission only once per tenant
            builder.HasIndex(up => new { up.UserId, up.PermissionId, up.TenantId })
                .IsUnique();

            // Relationships
            builder.HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
