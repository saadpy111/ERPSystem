using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities;

namespace Report.Persistence.Configurations
{
    public class ReportFilterConfiguration : IEntityTypeConfiguration<Report.Domain.Entities.ReportFilter>
    {
        public void Configure(EntityTypeBuilder<Report.Domain.Entities.ReportFilter> builder)
        {
            builder.HasKey(f => f.ReportFilterId);
            
            builder.Property(f => f.FieldName)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(f => f.Operator)
                .IsRequired();
                
            // Configure relationship
            builder.HasOne(f => f.Report)
                .WithMany(r => r.Filters)
                .HasForeignKey(f => f.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}