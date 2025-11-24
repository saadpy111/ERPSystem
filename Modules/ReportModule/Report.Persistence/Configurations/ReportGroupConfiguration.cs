using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities;

namespace Report.Persistence.Configurations
{
    public class ReportGroupConfiguration : IEntityTypeConfiguration<Report.Domain.Entities.ReportGroup>
    {
        public void Configure(EntityTypeBuilder<Report.Domain.Entities.ReportGroup> builder)
        {
            builder.HasKey(g => g.ReportGroupId);
            
            builder.Property(g => g.Expression)
                .IsRequired()
                .HasMaxLength(500);
                
            // Configure relationship
            builder.HasOne(g => g.Report)
                .WithMany(r => r.Groups)
                .HasForeignKey(g => g.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}