using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounting.Persistence.Configurations
{
    public class BudgetLineConfiguration : IEntityTypeConfiguration<BudgetLine>
    {
        public void Configure(EntityTypeBuilder<BudgetLine> builder)
        {
            builder.ToTable("BudgetLines");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.PlannedAmount).HasColumnType("decimal(18,6)");
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();

            builder.HasOne(e => e.Budget)
                .WithMany(e => e.Lines)
                .HasForeignKey(e => e.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Account)
                .WithMany()
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CostCenter)
                .WithMany()
                .HasForeignKey(e => e.CostCenterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.BudgetId);
            builder.HasIndex(e => e.AccountId);
            builder.HasIndex(e => e.CostCenterId);
            builder.HasIndex(e => e.TenantId);
        }
    }
}
