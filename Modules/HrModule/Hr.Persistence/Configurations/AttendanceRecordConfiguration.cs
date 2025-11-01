using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            builder.HasKey(ar => ar.RecordId);

            builder.Property(ar => ar.Date)
                .IsRequired();

            builder.HasOne(ar => ar.Employee)
                .WithMany(e => e.AttendanceRecords)
                .HasForeignKey(ar => ar.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
