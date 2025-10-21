using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class StockQuantConfiguration : IEntityTypeConfiguration<StockQuant>
    {
        public void Configure(EntityTypeBuilder<StockQuant> builder)
        {
            builder.ToTable("StockQuants");
            builder.HasKey(sq => sq.Id);

            builder.HasIndex(sq => new { sq.ProductId, sq.LocationId })
                   .IsUnique();

            builder.Property(sq => sq.Quantity).IsRequired();
            builder.Property(sq => sq.ReservedQuantity).IsRequired();

            builder.HasOne(sq => sq.Product)
                   .WithMany(p => p.StockQuants)
                   .HasForeignKey(sq => sq.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sq => sq.Location)
                   .WithMany(l => l.StockQuants)
                   .HasForeignKey(sq => sq.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
