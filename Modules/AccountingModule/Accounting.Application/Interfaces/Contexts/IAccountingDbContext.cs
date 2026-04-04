using Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Interfaces.Contexts
{
    public interface IAccountingDbContext
    {
        DbSet<Account> Accounts { get; }
        DbSet<JournalEntry> JournalEntries { get; }
        DbSet<JournalEntryLine> JournalEntryLines { get; }
        
        // Expose IQueryable for complex aggregations if preferred, or use DbSet directly
    }
}
