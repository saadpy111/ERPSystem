using Hr.Domain.Entities;
using Hr.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hr.Persistence.Configurations
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.HasKey(lt => lt.LeaveTypeId);
            
            builder.Property(lt => lt.LeaveTypeName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(lt => lt.Notes)
                .HasMaxLength(500);
                
            builder.Property(lt => lt.Status)
                .HasConversion<string>()
                .HasMaxLength(200)
                .HasDefaultValue(LeaveTypeStatus.NoPaid);
                
  
        }
    }
}