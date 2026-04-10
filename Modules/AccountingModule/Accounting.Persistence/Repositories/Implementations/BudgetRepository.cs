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
    public class BudgetRepository : GenericRepository<Budget>, IBudgetRepository
    {
        public BudgetRepository(AccountingDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Budget>> GetActiveBudgetsForDateAsync(DateTime date)
        {
            return await _context.Set<Budget>()
                .AsNoTracking()
                .Include(b => b.FiscalYear)
                .Include(b => b.Lines)
                .Where(b =>
                    b.Status == BudgetStatus.Active &&
                    b.FiscalYear.StartDate <= date &&
                    b.FiscalYear.EndDate >= date)
                .ToListAsync();
        }
    }

    public class BudgetLineRepository : GenericRepository<BudgetLine>, IBudgetLineRepository
    {
        public BudgetLineRepository(AccountingDbContext context) : base(context)
        {
        }
    }
}
