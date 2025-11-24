using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities;

namespace Report.Persistence.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report.Domain.Entities.Report>
    {
        public void Configure(EntityTypeBuilder<Report.Domain.Entities.Report> builder)
        {
            builder.HasKey(r => r.ReportId);
            
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            builder.Property(r => r.Description)
                .HasMaxLength(500);
                
            builder.Property(r => r.Query)
                .IsRequired();
                
            builder.Property(r => r.IsActive)
                .HasDefaultValue(true);
        }
    }
}