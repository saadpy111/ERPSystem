using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hr.Persistence.Context
{
    public class HrDbContext : DbContext
    {
        public HrDbContext(DbContextOptions<HrDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<RecruitmentStage> RecruitmentStages { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanInstallment> LoanInstallments { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }
        public DbSet<PayrollComponent> PayrollComponents { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema("Hr");
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
