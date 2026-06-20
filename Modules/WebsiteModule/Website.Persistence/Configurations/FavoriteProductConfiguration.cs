using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Website.Domain.Entities;

namespace Website.Persistence.Configurations
{
    public class FavoriteProductConfiguration : IEntityTypeConfiguration<FavoriteProduct>
    {
        public void Configure(EntityTypeBuilder<FavoriteProduct> builder)
        {
            builder.ToTable("FavoriteProducts");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(f => f.TenantId)
                .IsRequired();

            builder.HasOne(f => f.Product)
                .WithMany()
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(f => new { f.UserId, f.ProductId, f.TenantId })
                .IsUnique()
                .HasDatabaseName("IX_FavoriteProducts_UserId_ProductId_TenantId");

            builder.HasIndex(f => new { f.TenantId, f.UserId })
                .HasDatabaseName("IX_FavoriteProducts_TenantId_UserId");
        }
    }
}
