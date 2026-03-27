using Hr.Domain;
using Hr.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Multitenancy;
using System.Reflection;

namespace Hr.Persistence.Context
{
    public class HrDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        private string TenantId => _tenantProvider.GetTenantId()!;

        public HrDbContext(
            DbContextOptions<HrDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        #region DbSets

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<RecruitmentStage> RecruitmentStages { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ApplicantEducation> ApplicantEducations { get; set; }
        public DbSet<ApplicantExperience> ApplicantExperiences { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanInstallment> LoanInstallments { get; set; }
        public DbSet<PayrollRecord> PayrollRecords { get; set; }
        public DbSet<PayrollComponent> PayrollComponents { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<EmployeeContract> EmployeeContracts { get; set; }
        public DbSet<HrAttachment> Attachments { get; set; }
        public DbSet<SalaryStructure> SalaryStructures { get; set; }
        public DbSet<SalaryStructureComponent> SalaryStructureComponents { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Hr");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            ApplyGlobalTenantFilter(modelBuilder);
        }

        #region Global Filter

        private void ApplyGlobalTenantFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(HrDbContext)
                        .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder)
            where TEntity : BaseEntity
        {
            modelBuilder.Entity<TEntity>()
                .HasQueryFilter(e => e.TenantId == TenantId);
        }

        #endregion

        #region SaveChanges

        public override int SaveChanges()
        {
            ApplyTenantAndAudit();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTenantAndAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTenantAndAudit()
        {
            var tenantId = _tenantProvider.GetTenantId();

            if (string.IsNullOrEmpty(tenantId))
                throw new Exception("TenantId is not set!");

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.TenantId = tenantId;
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    if (entry.OriginalValues["TenantId"]?.ToString() != tenantId)
                        throw new Exception("Cross-tenant update is not allowed!");

                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    if (entry.OriginalValues["TenantId"]?.ToString() != tenantId)
                        throw new Exception("Cross-tenant delete is not allowed!");
                }
            }
        }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string con = "Server=DESKTOP-VGEBCK1\\SQLEXPRESS;Database=InventoryMicro;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True;";
                optionsBuilder.UseSqlServer(con);
            }
        }
    }
}