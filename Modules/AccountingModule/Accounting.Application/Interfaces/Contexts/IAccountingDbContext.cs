using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Interfaces.Contexts
{
    public interface IAccountingDbContext
    {
        DbSet<Account> Accounts { get; }
        DbSet<JournalEntry> JournalEntries { get; }
        DbSet<JournalEntryLine> JournalEntryLines { get; }
        DbSet<FiscalYear> FiscalYears { get; }
        DbSet<FiscalPeriod> FiscalPeriods { get; }
        DbSet<CostCenter> CostCenters { get; }
        DbSet<Partner> Partners { get; }
        
        // Expose IQueryable for complex aggregations if preferred, or use DbSet directly
        Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken);
    }
}
