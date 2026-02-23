using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(50);

            // Unique index for the code per tenant
            builder.HasIndex(c => new { c.TenantId, c.Code })
                .IsUnique();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.DiscountValue)
                .HasPrecision(18, 2);

            builder.Property(c => c.MinimumOrderAmount)
                .HasPrecision(18, 2);
        }
    }
}
