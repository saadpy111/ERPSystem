using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistense.Configurations
{
    public class TenantInvitationConfiguration : IEntityTypeConfiguration<TenantInvitation>
    {
        public void Configure(EntityTypeBuilder<TenantInvitation> builder)
        {
            builder.ToTable("TenantInvitations", "Identity");

            builder.HasKey(ti => ti.Id);

            builder.Property(ti => ti.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(ti => ti.InvitationToken)
                .IsRequired()
                .HasMaxLength(450);

            builder.HasIndex(ti => ti.InvitationToken)
                .IsUnique();

            builder.HasIndex(ti => ti.Email);

            builder.Property(ti => ti.Status)
                .IsRequired();

            builder.Property(ti => ti.ExpiresAt)
                .IsRequired();

            builder.Property(ti => ti.CreatedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(ti => ti.Tenant)
                .WithMany()
                .HasForeignKey(ti => ti.TenantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ti => ti.Role)
                .WithMany()
                .HasForeignKey(ti => ti.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ti => ti.InvitedByUser)
                .WithMany()
                .HasForeignKey(ti => ti.InvitedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ti => ti.AcceptedByUser)
                .WithMany()
                .HasForeignKey(ti => ti.AcceptedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
