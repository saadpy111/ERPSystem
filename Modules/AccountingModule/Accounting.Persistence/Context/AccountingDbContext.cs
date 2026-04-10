using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Common.Interfaces;
using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Multitenancy;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Accounting.Persistence.Context
{
    public class AccountingDbContext : DbContext, IAccountingDbContext
    {
        private readonly ITenantProvider? _tenantProvider;

        public string? CurrentTenantIdStr => _tenantProvider?.GetTenantId();
        
        public Guid? CurrentTenantId
        {
            get 
            {
                if (Guid.TryParse(CurrentTenantIdStr, out var tenantId))
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
        public DbSet<VoucherLine> VoucherLines { get; set; }
        public DbSet<CostCenter> CostCenters { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Payable>  Payables { get; set; }
        public DbSet<PayablePayment>  PayablePayments { get; set; }
        public DbSet<Receivable>  Receivables { get; set; }
        public DbSet<ReceivablePayment>  ReceivablePayments { get; set; }
        public DbSet<Sequence> Sequences { get; set; }
        public DbSet<AccountingMapping> AccountingMappings { get; set; }
        public DbSet<CashAccount> CashAccounts { get; set; }
        public DbSet<CashTransaction> CashTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.HasDefaultSchema("Accounting");

            ApplyGlobalQueryFilters(modelBuilder);
            ApplyGlobalConfigConventions(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ApplyGlobalConfigConventions(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // 1. Prevent Cascade Delete for all Foreign Keys
                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    // For accounting, strict Restrict prevents physical cascading deletion across relationships
                    if (foreignKey.DeleteBehavior == DeleteBehavior.Cascade || foreignKey.DeleteBehavior == DeleteBehavior.ClientCascade)
                    {
                        foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                    }
                }

                foreach (var property in entityType.GetProperties())
                {
                    // 2. Remove nvarchar(max) for Strings and apply sizing best practices
                    if (property.ClrType == typeof(string))
                    {
                        var name = property.Name;
                        if (name.Contains("Description", StringComparison.OrdinalIgnoreCase))
                            property.SetMaxLength(500);
                        else if (name.Contains("Note", StringComparison.OrdinalIgnoreCase))
                            property.SetMaxLength(1000);
                        else if (name.Contains("Secret", StringComparison.OrdinalIgnoreCase) || name.Contains("Token", StringComparison.OrdinalIgnoreCase))
                            property.SetMaxLength(1000);
                        else if (name.Contains("Code", StringComparison.OrdinalIgnoreCase) || name.Contains("Phone", StringComparison.OrdinalIgnoreCase))
                            property.SetMaxLength(50);
                        else if (name.Contains("Name", StringComparison.OrdinalIgnoreCase) || name.Contains("Email", StringComparison.OrdinalIgnoreCase))
                            property.SetMaxLength(200);
                        else if (property.GetMaxLength() == null)
                            property.SetMaxLength(200); // Default bounded max-length preventing nvarchar(max)
                    }

                    // Bonus: 6. Conform to rigorous decimal precision (18, 6) across the board structurally
                    if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
                    {
                        property.SetPrecision(18);
                        property.SetScale(6);
                    }
                }
            }
        }

        private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var filter = GlobalQueryFilterBuilder.BuildFilterExpression(entityType.ClrType, this);
                if (filter != null)
                {
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
        }

        public override int SaveChanges()
        {
            ApplyTenantAndAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTenantAndAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTenantAndAudit()
        {
            var tenantId = CurrentTenantId;
            var now = DateTime.UtcNow;
            const string systemUser = "system";

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IMultiTenant multiTenant && entry.State == EntityState.Added && tenantId.HasValue)
                {
                    multiTenant.TenantId = tenantId.Value;
                }

                if (entry.Entity is IAuditable auditable)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedAt = now;
                        if (string.IsNullOrWhiteSpace(auditable.CreatedBy))
                        {
                            auditable.CreatedBy = systemUser;
                        }
                        auditable.UpdatedAt = null;
                        auditable.UpdatedBy = null;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        auditable.UpdatedAt = now;
                        if (string.IsNullOrWhiteSpace(auditable.UpdatedBy))
                        {
                            auditable.UpdatedBy = systemUser;
                        }
                    }
                }
            }
        }
    }

    internal static class GlobalQueryFilterBuilder
    {
        public static LambdaExpression? BuildFilterExpression(Type entityType, AccountingDbContext dbContext)
        {
            var parameter = Expression.Parameter(entityType, "e");
            Expression? filterBody = null;

            if (typeof(IMultiTenant).IsAssignableFrom(entityType))
            {
                var tenantProperty = Expression.Property(parameter, nameof(IMultiTenant.TenantId));
                var currentTenant = Expression.Property(Expression.Constant(dbContext), nameof(AccountingDbContext.CurrentTenantId));
                var hasTenant = Expression.Property(currentTenant, nameof(Nullable<Guid>.HasValue));
                var tenantValue = Expression.Property(currentTenant, nameof(Nullable<Guid>.Value));
                var tenantMatch = Expression.Equal(tenantProperty, tenantValue);
                var tenantCondition = Expression.OrElse(Expression.Not(hasTenant), tenantMatch);

                filterBody = tenantCondition;
            }

            if (typeof(ISoftDelete).IsAssignableFrom(entityType))
            {
                var isDeletedProperty = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                var notDeleted = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                filterBody = filterBody == null ? notDeleted : Expression.AndAlso(filterBody, notDeleted);
            }

            return filterBody == null
                ? null
                : Expression.Lambda(filterBody, parameter);
        }
    }
}
