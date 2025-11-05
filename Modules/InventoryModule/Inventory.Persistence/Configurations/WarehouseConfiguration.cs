using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.HasKey(w => w.Id);
            builder.Property(w => w.Name).HasMaxLength(100).IsRequired();
            builder.Property(w => w.WarehouseType).HasMaxLength(100);
            builder.Property(w => w.ResponsibleEmployee).HasMaxLength(100);
            builder.Property(w => w.WarehouseCode).HasMaxLength(100);
            builder.Property(w => w.ContactNumber).HasMaxLength(100);
            builder.Property(w => w.FinancialAccountCode).HasMaxLength(100);
            builder.Property(w => w.InventoryPolicy).HasMaxLength(100);
        }
    }
}
