using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class CustomerProfileConfiguration : IEntityTypeConfiguration<CustomerProfile>
    {
        public void Configure(EntityTypeBuilder<CustomerProfile> builder)
        {
            builder.ToTable("CustomerProfiles");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(c => c.AvatarUrl)
                .HasMaxLength(1000);

            builder.Property(c => c.Address)
                .HasMaxLength(500);

            builder.Property(c => c.City)
                .HasMaxLength(200);

            builder.Property(c => c.Country)
                .HasMaxLength(200);

            builder.Property(c => c.PostalCode)
                .HasMaxLength(20);

            builder.Property(c => c.AdminNotes)
                .HasMaxLength(2000);

            // Index on UserId for fast lookups
            builder.HasIndex(c => c.UserId);

            // Unique constraint: one profile per user per tenant
            builder.HasIndex(c => new { c.TenantId, c.UserId })
                .IsUnique();
        }
    }
}
