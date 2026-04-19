using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Inventory.Persistence.Configurations
{
    // 1. Product
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.Sku)
                   .IsUnique();
            builder
               .HasIndex(e => e.TenantId);

            builder.Property(p => p.Sku).HasMaxLength(50);
            builder.Property(p => p.ProductBarcode).HasMaxLength(250);
            builder.Property(p => p.MainSupplierName).HasMaxLength(250);
            builder.Property(p => p.Name).HasMaxLength(255).IsRequired();

            // POS & weighted product fields
            builder.Property(p => p.ProductType)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .HasDefaultValue(Inventory.Domain.Enums.ProductType.FinishedGood);

            builder.Property(p => p.IsSellableInPOS)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(p => p.IsWeighted)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(p => p.PricePerKg)
                   .HasColumnType("decimal(18,4)");

            builder.Property(p => p.DefaultTareWeight)
                   .HasColumnType("decimal(18,4)");

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
