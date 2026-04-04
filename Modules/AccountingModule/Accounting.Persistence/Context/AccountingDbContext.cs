using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Multitenancy;
using System.Reflection;

namespace Accounting.Persistence.Context
{
    public class AccountingDbContext : DbContext
    {
        private readonly ITenantProvider? _tenantProvider;

        public string? CurrentTenantIdStr => _tenantProvider?.GetTenantId();
        
        public int? CurrentTenantId 
        {
            get 
            {
                if (int.TryParse(CurrentTenantIdStr, out int tenantId))
                {
                    return tenantId;
                }
                return null;
            }
        }

        public AccountingDbContext(
            DbContextOptions<AccountingDbContext> options,
            ITenantProvider? tenantProvider = null) : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<FiscalPeriod> FiscalPeriods { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<JournalEntryLine> JournalEntryLines { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Sequence> Sequences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema("Accounting");

            ApplyGlobalQueryFilters(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            if (CurrentTenantId.HasValue)
            {
                var tenantId = CurrentTenantId.Value;

                modelBuilder.Entity<Account>().HasQueryFilter(e => e.TenantId == tenantId && !e.IsDeleted);
                modelBuilder.Entity<Currency>().HasQueryFilter(e => e.TenantId == tenantId);
                modelBuilder.Entity<CurrencyRate>().HasQueryFilter(e => e.Currency.TenantId == tenantId); // Indirect, but useful if needed. Wait, CurrencyRate doesn't have TenantId directly.
                modelBuilder.Entity<Partner>().HasQueryFilter(e => e.TenantId == tenantId && !e.IsDeleted);
                modelBuilder.Entity<FiscalYear>().HasQueryFilter(e => e.TenantId == tenantId);
                // FiscalPeriod doesn't have TenantId directly in our definition, filtering through FiscalYear could be complex for EF query filters, so we skip or enforce it properly elsewhere. 
                // Wait, if no TenantId on FiscalPeriod, better skip its query filter and rely on FiscalYear.
                modelBuilder.Entity<JournalEntry>().HasQueryFilter(e => e.TenantId == tenantId && !e.IsDeleted);
                modelBuilder.Entity<JournalEntryLine>().HasQueryFilter(e => e.TenantId == tenantId);
                modelBuilder.Entity<Voucher>().HasQueryFilter(e => e.TenantId == tenantId);
                modelBuilder.Entity<CostCenter>().HasQueryFilter(e => e.TenantId == tenantId);
                modelBuilder.Entity<Tax>().HasQueryFilter(e => e.TenantId == tenantId);
                modelBuilder.Entity<Sequence>().HasQueryFilter(e => e.TenantId == tenantId);
            }
            else
            {
                // Soft deletes for when tenant is not resolved
                modelBuilder.Entity<Account>().HasQueryFilter(e => !e.IsDeleted);
                modelBuilder.Entity<Partner>().HasQueryFilter(e => !e.IsDeleted);
                modelBuilder.Entity<JournalEntry>().HasQueryFilter(e => !e.IsDeleted);
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            EnforceTenantOnInsert();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void EnforceTenantOnInsert()
        {
            var tenantId = CurrentTenantId;
            if (!tenantId.HasValue) return;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    var property = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "TenantId");
                    if (property != null && property.Metadata.ClrType == typeof(int))
                    {
                        property.CurrentValue = tenantId.Value;
                    }
                }
            }
        }
    }
}
