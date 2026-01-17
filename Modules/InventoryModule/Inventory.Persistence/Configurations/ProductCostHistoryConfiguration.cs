using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class ProductCostHistoryConfiguration : IEntityTypeConfiguration<ProductCostHistory>
    {
        public void Configure(EntityTypeBuilder<ProductCostHistory> builder)
        {
            builder.ToTable("ProductCostHistories");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.OldCost).HasColumnType("decimal(18, 4)").IsRequired();
            builder.Property(c => c.NewCost).HasColumnType("decimal(18, 4)").IsRequired();

            builder.HasIndex(c => c.TenantId);
            builder.HasOne(c => c.Product)
                   .WithMany(p => p.CostHistories)
                   .HasForeignKey(c => c.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.StockMove)
                   .WithMany()
                   .HasForeignKey(c => c.StockMoveId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
