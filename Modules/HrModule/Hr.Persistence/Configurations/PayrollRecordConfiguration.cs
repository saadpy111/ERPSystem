using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class PayrollRecordConfiguration : IEntityTypeConfiguration<PayrollRecord>
    {
        public void Configure(EntityTypeBuilder<PayrollRecord> builder)
        {
            builder.HasKey(pr => pr.PayrollId);

            builder.Property(pr => pr.BaseSalary)
                .HasColumnType("decimal(18,2)");

            builder.Property(pr => pr.TotalAllowances)
                .HasColumnType("decimal(18,2)");

            builder.Property(pr => pr.TotalDeductions)
                .HasColumnType("decimal(18,2)");

            builder.Property(pr => pr.TotalGrossSalary)
                .HasColumnType("decimal(18,2)");

            builder.Property(pr => pr.NetSalary)
                .HasColumnType("decimal(18,2)");

            builder.Property(pr => pr.Status)
                .HasConversion<string>();

            builder.HasOne(pr => pr.Employee)
                .WithMany(e => e.PayrollRecords)
                .HasForeignKey(pr => pr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(pr => pr.Components)
                .WithOne(pc => pc.PayrollRecord)
                .HasForeignKey(pc => pc.PayrollRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
