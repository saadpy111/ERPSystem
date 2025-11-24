using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities;

namespace Report.Persistence.Configurations
{
    public class ReportParameterConfiguration : IEntityTypeConfiguration<Report.Domain.Entities.ReportParameter>
    {
        public void Configure(EntityTypeBuilder<Report.Domain.Entities.ReportParameter> builder)
        {
            builder.HasKey(p => p.ReportParameterId);
            
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(p => p.DisplayName)
                .HasMaxLength(200);
                
            builder.Property(p => p.DefaultValue)
                .HasMaxLength(500);
                
            // Configure relationship
            builder.HasOne(p => p.Report)
                .WithMany(r => r.Parameters)
                .HasForeignKey(p => p.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}