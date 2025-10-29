using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Procurement.Domain.Entities;

namespace Procurement.Persistence.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasKey(v => v.Id);
            
            builder.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(v => v.ContactName)
                .HasMaxLength(200);
                
            builder.Property(v => v.Phone)
                .HasMaxLength(20);
                
            builder.Property(v => v.Email)
                .HasMaxLength(100);
                
            builder.Property(v => v.Address)
                .HasMaxLength(500);
                
            builder.Property(v => v.TaxNumber)
                .HasMaxLength(50);
                
            builder.Property(v => v.IsActive)
                .HasDefaultValue(true);
                
            // Configure relationships
            builder.HasMany(v => v.PurchaseOrders)
                .WithOne(po => po.Vendor)
                .HasForeignKey(po => po.VendorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}