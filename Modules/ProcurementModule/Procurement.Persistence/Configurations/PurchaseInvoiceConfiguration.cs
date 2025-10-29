using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Persistence.Configurations
{
    public class PurchaseInvoiceConfiguration : IEntityTypeConfiguration<PurchaseInvoice>
    {
        public void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
        {
            builder.HasKey(pi => pi.Id);
            
            builder.Property(pi => pi.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(pi => pi.TotalAmount)
                .HasPrecision(18, 2);
                
            builder.Property(pi => pi.PaymentStatus)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(pi => pi.Notes)
                .HasMaxLength(500);
                
            // Configure relationships
            builder.HasOne(pi => pi.PurchaseOrder)
                .WithMany(po => po.Invoices)
                .HasForeignKey(pi => pi.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}