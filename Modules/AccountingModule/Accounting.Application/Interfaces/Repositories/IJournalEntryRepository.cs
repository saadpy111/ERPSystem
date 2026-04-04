using Accounting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IJournalEntryRepository : IGenericRepository<JournalEntry>
    {
        Task<IEnumerable<JournalEntry>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<JournalEntry>> GetPostedEntriesAsync();
    }
}
