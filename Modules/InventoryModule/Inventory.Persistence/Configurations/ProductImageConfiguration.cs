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

            builder.Property(pi => pi.ThumbnailUrl)
                .HasMaxLength(512);

            builder.Property(pi => pi.Description)
                .HasMaxLength(1024);

            builder.Property(pi => pi.IsPrimary)
                .HasDefaultValue(false);

            builder.Property(pi => pi.DisplayOrder)
                .HasDefaultValue(0);

            // store enum as string for readability
            builder.Property(pi => pi.ImageType)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();


            // use WithMany() to avoid requiring a navigation collection on Product
            builder.HasOne(p => p.Product)
                   .WithMany(p => p.Images)
                   .HasForeignKey(pi => pi.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(pi => pi.ProductId);
        }
    }
}