using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class InventoryQuarantineConfiguration : IEntityTypeConfiguration<InventoryQuarantine>
    {
        public void Configure(EntityTypeBuilder<InventoryQuarantine> builder)
        {
            builder.ToTable("InventoryQuarantines");
            builder.HasKey(iq => iq.Id);

            builder.Property(iq => iq.Status)
                   .HasConversion<string>() 
                   .HasMaxLength(50)
                   .IsRequired(); builder.Property(iq => iq.SourceReference).HasMaxLength(100);

            builder.HasOne(iq => iq.Product)
                   .WithMany()
                   .HasForeignKey(iq => iq.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(iq => iq.Location)
                   .WithMany()
                   .HasForeignKey(iq => iq.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
