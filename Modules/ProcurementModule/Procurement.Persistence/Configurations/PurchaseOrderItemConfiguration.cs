using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Persistence.Configurations
{
    public class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
            builder.HasKey(poi => poi.Id);
            
            builder.Property(poi => poi.ProductId)
                .IsRequired();
                
            builder.Property(poi => poi.UnitPrice)
                .HasPrecision(18, 2);
                
            builder.Property(poi => poi.Total)
                .HasPrecision(18, 2)
                .HasComputedColumnSql("[Quantity] * [UnitPrice]");


            // Configure relationships
            builder.HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(poi => poi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}