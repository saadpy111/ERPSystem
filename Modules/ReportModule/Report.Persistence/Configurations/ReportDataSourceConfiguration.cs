using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.Entities;

namespace Report.Persistence.Configurations
{
    public class ReportDataSourceConfiguration : IEntityTypeConfiguration<Report.Domain.Entities.ReportDataSource>
    {
        public void Configure(EntityTypeBuilder<Report.Domain.Entities.ReportDataSource> builder)
        {
            builder.HasKey(ds => ds.ReportDataSourceId);
            
            builder.Property(ds => ds.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(ds => ds.ConnectionString)
                .IsRequired()
                .HasMaxLength(500);
                
            builder.Property(ds => ds.SqlTemplate)
                .HasMaxLength(1000);
                
            builder.Property(ds => ds.Type)
                .IsRequired();
        }
    }
}