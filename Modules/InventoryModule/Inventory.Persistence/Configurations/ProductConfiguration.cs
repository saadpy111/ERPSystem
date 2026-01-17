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

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
