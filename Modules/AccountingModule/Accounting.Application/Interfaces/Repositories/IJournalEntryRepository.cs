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

        /// <summary>
        /// Loads a journal entry together with its <see cref="JournalEntryLine"/> collection
        /// and the linked <see cref="FiscalPeriod"/> in a single query.
        /// Required by the reversal handler to validate fiscal-period status and clone lines.
        /// </summary>
        Task<JournalEntry?> GetByIdWithLinesAsync(int id);
    }
}
