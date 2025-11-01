using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(l => l.LoanId);

            builder.Property(l => l.PrincipalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.MonthlyInstallment)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.RemainingBalance)
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(l => l.Employee)
                .WithMany(e => e.Loans)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(l => l.Installments)
                .WithOne(i => i.Loan)
                .HasForeignKey(i => i.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
