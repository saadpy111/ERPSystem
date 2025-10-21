using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class ProductAttributeValueConfiguration : IEntityTypeConfiguration<ProductAttributeValue>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeValue> builder)
        {
            builder.ToTable("ProductAttributeValues");
            builder.HasKey(pav => pav.Id);

            builder.Property(pav => pav.Value).HasMaxLength(500).IsRequired();

            builder.HasIndex(pav => new { pav.ProductId, pav.AttributeId });
               

            builder.HasOne(pav => pav.Product)
                   .WithMany(p => p.AttributeValues)
                   .HasForeignKey(pav => pav.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pav => pav.Attribute)
                   .WithMany(a => a.Values)
                   .HasForeignKey(pav => pav.AttributeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
