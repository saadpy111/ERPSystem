using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "Website");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(o => o.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(o => o.CustomerName)
                .HasMaxLength(200);

            builder.Property(o => o.CustomerPhone)
                .HasMaxLength(50);

            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(o => o.SubTotal)
                .HasPrecision(18, 2);

            builder.Property(o => o.DiscountTotal)
                .HasPrecision(18, 2);

            builder.Property(o => o.CouponDiscountAmount)
                .HasPrecision(18, 2);

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(o => o.Notes)
                .HasMaxLength(1000);

            builder.Property(o => o.PaymentId)
                .HasMaxLength(450);

            builder.Property(o => o.TenantId)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(o => o.TenantId);
            builder.HasIndex(o => o.UserId);
            builder.HasIndex(o => o.PaymentId);
            builder.HasIndex(o => o.OrderNumber);
        }
    }
}
