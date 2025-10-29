using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Persistence.Configurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.HasKey(po => po.Id);
            
            builder.Property(po => po.Status)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(po => po.TotalAmount)
                .HasPrecision(18, 2);
                
            builder.Property(po => po.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);
                
            // Configure relationships
            builder.HasOne(po => po.Vendor)
                .WithMany(v => v.PurchaseOrders)
                .HasForeignKey(po => po.VendorId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(po => po.Items)
                .WithOne(poi => poi.PurchaseOrder)
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(po => po.GoodsReceipts)
                .WithOne(gr => gr.PurchaseOrder)
                .HasForeignKey(gr => gr.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasMany(po => po.Invoices)
                .WithOne(pi => pi.PurchaseOrder)
                .HasForeignKey(pi => pi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}