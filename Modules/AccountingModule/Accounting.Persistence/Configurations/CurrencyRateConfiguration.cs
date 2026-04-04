using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class CurrencyRateConfiguration : IEntityTypeConfiguration<CurrencyRate>
    {
        public void Configure(EntityTypeBuilder<CurrencyRate> builder)
        {
            builder.ToTable("CurrencyRates");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.BuyRate).HasColumnType("decimal(18,6)");
            builder.Property(e => e.SellRate).HasColumnType("decimal(18,6)");
            builder.Property(e => e.OfficialRate).HasColumnType("decimal(18,6)");
            builder.Property(e => e.Source).HasMaxLength(100).IsRequired();

            builder.HasOne(e => e.Currency)
                .WithMany(e => e.CurrencyRates)
                .HasForeignKey(e => e.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.CurrencyId);
        }
    }
}
