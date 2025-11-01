using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class LoanInstallmentConfiguration : IEntityTypeConfiguration<LoanInstallment>
    {
        public void Configure(EntityTypeBuilder<LoanInstallment> builder)
        {
            builder.HasKey(li => li.InstallmentId);

            builder.Property(li => li.AmountDue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(li => li.PaymentMethod)
                .HasMaxLength(50);

            builder.Property(li => li.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(li => li.Loan)
                .WithMany(l => l.Installments)
                .HasForeignKey(li => li.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
