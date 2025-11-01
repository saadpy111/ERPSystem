using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.HasKey(lr => lr.RequestId);

            builder.Property(lr => lr.LeaveType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(lr => lr.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(lr => lr.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(lr => lr.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
