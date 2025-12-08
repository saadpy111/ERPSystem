using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities;

namespace Report.Persistence.Configurations
{
    public class InventoryReportConfiguration : IEntityTypeConfiguration<InventoryReport>
    {
        public void Configure(EntityTypeBuilder<InventoryReport> builder)
        {
            builder.HasKey(ir => ir.InventoryReportId);
            
            builder.Property(ir => ir.ProductName)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(ir => ir.Sku)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(ir => ir.Barcode)
                .HasMaxLength(100);
                
            builder.Property(ir => ir.CategoryName)
                .HasMaxLength(100);
                
            builder.Property(ir => ir.WarehouseName)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(ir => ir.LocationName)
                .HasMaxLength(200);


            builder.Property(ir => ir.Description)
                .HasMaxLength(500);


            builder.Property(ir => ir.MainSupplierName)
                .HasMaxLength(200);


            builder.Property(ir => ir.LastStockMoveType)
                .HasMaxLength(50);
                
            builder.Property(ir => ir.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}