using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.EmployeeId);

            builder.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(50);

            builder.Property(e => e.JobTitle)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.BaseSalary)
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
