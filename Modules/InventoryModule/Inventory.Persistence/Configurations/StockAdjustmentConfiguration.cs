using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class StockAdjustmentConfiguration : IEntityTypeConfiguration<StockAdjustment>
    {
        public void Configure(EntityTypeBuilder<StockAdjustment> builder)
        {
            builder.ToTable("StockAdjustments");
            builder.HasKey(sa => sa.Id);

            builder.Property(sa => sa.UserId).IsRequired();

            builder.HasOne(sa => sa.Warehouse)
                   .WithMany(w => w.StockAdjustments)
                   .HasForeignKey(sa => sa.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sa => sa.Product)
                   .WithMany(p => p.StockAdjustments)
                   .HasForeignKey(sa => sa.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
