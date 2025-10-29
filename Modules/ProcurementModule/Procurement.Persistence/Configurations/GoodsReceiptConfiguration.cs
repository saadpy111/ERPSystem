using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Persistence.Configurations
{
    public class GoodsReceiptConfiguration : IEntityTypeConfiguration<GoodsReceipt>
    {
        public void Configure(EntityTypeBuilder<GoodsReceipt> builder)
        {
            builder.HasKey(gr => gr.Id);
            
            builder.Property(gr => gr.ReceivedBy)
                .IsRequired()
                .HasMaxLength(100);
                
            //builder.Property(gr => gr.Status)
            //    .IsRequired()
            //    .HasMaxLength(50);
                


            builder.Property(gr => gr.Remarks)
                .HasMaxLength(500);
                
            // Configure relationships
            builder.HasOne(gr => gr.PurchaseOrder)
                .WithMany(po => po.GoodsReceipts)
                .HasForeignKey(gr => gr.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}