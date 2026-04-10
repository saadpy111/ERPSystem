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

        /// <summary>
        /// Returns the SUM of BaseAmount for all POSTED journal entry lines that match
        /// the given account, cost center and fall within the specified date range.
        /// Used by BudgetControlService to calculate actuals efficiently without
        /// loading navigation properties.
        /// </summary>
        Task<decimal> GetActualAmountAsync(int accountId, int? costCenterId, DateTime startDate, DateTime endDate);
    }
}
