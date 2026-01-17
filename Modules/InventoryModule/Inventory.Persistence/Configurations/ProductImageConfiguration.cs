using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasMaxLength(512);

            builder.HasIndex(pi => pi.TenantId);
            builder.Property(pi => pi.Description)
                .HasMaxLength(1024);

            builder.Property(pi => pi.IsPrimary)
                .HasDefaultValue(false);

            builder.Property(pi => pi.DisplayOrder)
                .HasDefaultValue(0);




            // use WithMany() to avoid requiring a navigation collection on Product
            builder.HasOne(p => p.Product)
                   .WithMany(p => p.Images)
                   .HasForeignKey(pi => pi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(pi => pi.ProductId);
        }
    }
}