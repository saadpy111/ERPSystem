using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Persistence.Configurations
{
    public class GoodsReceiptItemConfiguration : IEntityTypeConfiguration<GoodsReceiptItem>
    {
        public void Configure(EntityTypeBuilder<GoodsReceiptItem> builder)
        {
            builder.HasKey(gri => gri.Id);
            
            builder.Property(gri => gri.ProductId)
                .IsRequired();
                
            builder.Property(gri => gri.ReceivedQuantity)
                .IsRequired();
                
            builder.Property(gri => gri.UnitPrice)
                .HasColumnType("decimal(18,2)");
                
            builder.Property(gri => gri.Remarks)
                .HasMaxLength(500);
                
            // Configure relationships
            builder.HasOne(gri => gri.GoodsReceipt)
                .WithMany(gr => gr.Items)
                .HasForeignKey(gri => gri.GoodsReceiptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}