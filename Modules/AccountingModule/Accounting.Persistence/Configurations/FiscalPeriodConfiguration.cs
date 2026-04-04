using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class FiscalPeriodConfiguration : IEntityTypeConfiguration<FiscalPeriod>
    {
        public void Configure(EntityTypeBuilder<FiscalPeriod> builder)
        {
            builder.ToTable("FiscalPeriods");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.PeriodName).HasMaxLength(100).IsRequired();

            builder.HasOne(e => e.FiscalYear)
                .WithMany(e => e.FiscalPeriods)
                .HasForeignKey(e => e.FiscalYearId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.FiscalYearId);
        }
    }
}
