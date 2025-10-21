using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");

            builder.HasKey(l => l.Id);
            builder.Property(l => l.Name).HasMaxLength(100).IsRequired();
            builder.Property(l => l.Type).HasMaxLength(50); 

            builder.HasOne(l => l.Warehouse)
                   .WithMany(w => w.Locations)
                   .HasForeignKey(l => l.WarehouseId)
                   .OnDelete(DeleteBehavior.Cascade);

            
            builder.HasOne(l => l.Parent)
                   .WithMany(l => l.ChildLocations)
                   .HasForeignKey(l => l.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
