using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class StockMoveConfiguration : IEntityTypeConfiguration<StockMove>
    {
        public void Configure(EntityTypeBuilder<StockMove> builder)
        {
            builder.ToTable("StockMoves");
            builder.HasKey(sm => sm.Id);

            builder.Property(sm => sm.MoveType)
                   .IsRequired()
                   .HasConversion<string>()   
                   .HasMaxLength(50);
            builder.HasIndex(sm => sm.TenantId);

            builder.Property(sm => sm.Reference)
                   .HasMaxLength(100);

            builder.Property(sm => sm.Quantity)
                   .IsRequired();

            builder.Property(sm => sm.MoveDate)
                   .IsRequired();

            builder.HasOne(sm => sm.Product)
                   .WithMany(p => p.StockMoves)
                   .HasForeignKey(sm => sm.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.SourceLocation)
                   .WithMany()
                   .HasForeignKey(sm => sm.SourceLocationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sm => sm.DestinationLocation)
                   .WithMany()
                   .HasForeignKey(sm => sm.DestinationLocationId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
