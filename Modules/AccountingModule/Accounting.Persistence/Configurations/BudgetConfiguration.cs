using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("Budgets");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).HasMaxLength(200).IsRequired();
            builder.Property(e => e.Status).IsRequired();

            builder.HasOne(e => e.FiscalYear)
                .WithMany()
                .HasForeignKey(e => e.FiscalYearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.TenantId);
            builder.HasIndex(e => e.FiscalYearId);
        }
    }
}
