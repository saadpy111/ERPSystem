using Microsoft.EntityFrameworkCore;
using Report.Domain.Entities;
using System.Reflection;

namespace Report.Persistence.Context
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
        {
        }

        public DbSet<Report.Domain.Entities.Report> Reports { get; set; }
        public DbSet<Report.Domain.Entities.ReportDataSource> ReportDataSources { get; set; }
        public DbSet<Report.Domain.Entities.ReportField> ReportFields { get; set; }
        public DbSet<Report.Domain.Entities.ReportParameter> ReportParameters { get; set; }
        public DbSet<Report.Domain.Entities.ReportFilter> ReportFilters { get; set; }
        public DbSet<Report.Domain.Entities.ReportGroup> ReportGroups { get; set; }
        public DbSet<Report.Domain.Entities.ReportSorting> ReportSortings { get; set; }
        public DbSet<EmployeeReport> EmployeeReports { get; set; }
        public DbSet<InventoryReport> InventoryReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema("Report");
        }
    }
}