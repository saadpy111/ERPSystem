using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities;

namespace Report.Persistence.Configurations
{
    public class ReportFieldConfiguration : IEntityTypeConfiguration<Report.Domain.Entities.ReportField>
    {
        public void Configure(EntityTypeBuilder<Report.Domain.Entities.ReportField> builder)
        {
            builder.HasKey(f => f.ReportFieldId);
            
            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(f => f.DisplayName)
                .HasMaxLength(200);
                
            builder.Property(f => f.Expression)
                .HasMaxLength(5000)
                .IsRequired();
                
            builder.Property(f => f.Type)
                .IsRequired();
                
            // Configure relationship
            builder.HasOne(f => f.Report)
                .WithMany(r => r.Fields)
                .HasForeignKey(f => f.ReportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}