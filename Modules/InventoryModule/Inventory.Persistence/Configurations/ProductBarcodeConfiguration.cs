using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class ProductBarcodeConfiguration : IEntityTypeConfiguration<ProductBarcode>
    {
        public void Configure(EntityTypeBuilder<ProductBarcode> builder)
        {
            builder.ToTable("ProductBarcodes");
            builder.HasKey(b => b.Id);

            builder.HasIndex(b => b.TenantId);
            builder.Property(b => b.BarcodeValue).HasMaxLength(150).IsRequired();
            builder.Property(b => b.Type).HasMaxLength(50);

            builder.HasIndex(b => b.BarcodeValue).IsUnique();

            builder.Property(b => b.IsWeighted)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.Property(b => b.Prefix)
                   .HasMaxLength(10);

            builder.HasOne(b => b.Product)
                   .WithMany(p => p.Barcodes)
                   .HasForeignKey(b => b.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
