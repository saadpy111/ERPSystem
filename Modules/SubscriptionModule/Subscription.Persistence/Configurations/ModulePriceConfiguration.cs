using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription.Domain.Entities;

namespace Subscription.Persistence.Configurations
{
    public class ModulePriceConfiguration : IEntityTypeConfiguration<ModulePrice>
    {
        public void Configure(EntityTypeBuilder<ModulePrice> builder)
        {
            builder.ToTable("ModulePrices", "Subscription");

            builder.HasKey(mp => mp.Id);

            builder.Property(mp => mp.CurrencyCode)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(mp => mp.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(mp => new { mp.ModuleId, mp.CurrencyCode, mp.Interval })
                .IsUnique();

            builder.HasIndex(mp => mp.IsActive);

            builder.HasOne(mp => mp.Module)
                .WithMany()
                .HasForeignKey(mp => mp.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
