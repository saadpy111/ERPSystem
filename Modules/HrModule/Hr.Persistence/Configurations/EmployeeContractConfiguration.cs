using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class EmployeeContractConfiguration : IEntityTypeConfiguration<EmployeeContract>
    {
        public void Configure(EntityTypeBuilder<EmployeeContract> builder)
        {
            builder.HasKey(ec => ec.Id);

            builder.Property(ec => ec.Salary)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(ec => ec.ContractType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(ec => ec.Notes)
                .HasMaxLength(1000);

            builder.HasOne(ec => ec.Employee)
                .WithMany(e => e.Contracts)
                .HasForeignKey(ec => ec.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ec => ec.Job)
                .WithMany()
                .HasForeignKey(ec => ec.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ec => ec.SalaryStructure)
                .WithMany()
                .HasForeignKey(ec => ec.SalaryStructureId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}