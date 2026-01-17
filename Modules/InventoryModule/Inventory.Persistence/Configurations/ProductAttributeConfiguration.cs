using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> builder)
        {
            builder.ToTable("ProductAttributes");
            builder.HasKey(pa => pa.Id);

            builder.HasIndex(pa => pa.TenantId);
            builder.Property(pa => pa.Name).HasMaxLength(100).IsRequired();
            builder.Property(pa => pa.DataType).HasMaxLength(50).IsRequired();
        }
    }
}
