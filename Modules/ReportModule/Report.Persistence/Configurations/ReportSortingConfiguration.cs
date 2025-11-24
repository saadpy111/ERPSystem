using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities;
using Report.Domain.Enums;

namespace Report.Persistence.Configurations
{
    public class ReportSortingConfiguration : IEntityTypeConfiguration<Report.Domain.Entities.ReportSorting>
    {
        public void Configure(EntityTypeBuilder<Report.Domain.Entities.ReportSorting> builder)
        {
            builder.HasKey(s => s.ReportSortingId);
            
            builder.Property(s => s.Expression)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(s => s.Direction)
                .HasDefaultValue(SortDirection.Ascending);
                
            // Configure relationship
            builder.HasOne(s => s.Report)
                .WithMany(r => r.Sortings)
                .HasForeignKey(s => s.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}