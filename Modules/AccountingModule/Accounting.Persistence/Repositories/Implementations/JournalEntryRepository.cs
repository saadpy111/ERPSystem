using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Domain.Enums;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class JournalEntryRepository : GenericRepository<JournalEntry>, IJournalEntryRepository
    {
        public JournalEntryRepository(AccountingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<JournalEntry>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(j => j.Lines)
                .Where(j => j.Date >= startDate && j.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<JournalEntry>> GetPostedEntriesAsync()
        {
            return await _dbSet
                .Include(j => j.Lines)
                .Where(j => j.Status == JournalStatus.Posted)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<JournalEntry?> GetByIdWithLinesAsync(int id)
        {
            return await _dbSet
                .Include(j => j.Lines)
                .Include(j => j.FiscalPeriod)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        /// <inheritdoc/>
        public async Task<decimal> GetActualAmountAsync(
            int accountId,
            int? costCenterId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.Set<JournalEntryLine>()
                .AsNoTracking()
                .Where(l =>
                    l.JournalEntry.Status == JournalStatus.Posted &&
                    l.AccountId == accountId &&
                    l.CostCenterId == costCenterId &&
                    l.JournalEntry.Date >= startDate &&
                    l.JournalEntry.Date <= endDate)
                .SumAsync(l => (decimal?)l.BaseAmount) ?? 0m;
        }
    }
}

