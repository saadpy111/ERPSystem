using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class SerialOrBatchNumberConfiguration : IEntityTypeConfiguration<SerialOrBatchNumber>
    {
        public void Configure(EntityTypeBuilder<SerialOrBatchNumber> builder)
        {
            builder.ToTable("SerialOrBatchNumbers");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Number).HasMaxLength(150).IsRequired();
            builder.Property(s => s.Quantity).IsRequired();

            builder.HasIndex(s => s.Number).IsUnique();
            builder.HasIndex(s => s.TenantId);
            builder.HasOne(s => s.Product)
                   .WithMany(p => p.SerialNumbers)
                   .HasForeignKey(s => s.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.CurrentLocation)
                   .WithMany()
                   .HasForeignKey(s => s.CurrentLocationId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
