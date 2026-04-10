using Accounting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface IBudgetRepository : IGenericRepository<Budget>
    {
        /// <summary>
        /// Returns all Active budgets whose FiscalYear contains <paramref name="date"/>,
        /// with FiscalYear and BudgetLines eagerly loaded.
        /// </summary>
        Task<IEnumerable<Budget>> GetActiveBudgetsForDateAsync(DateTime date);
    }

    public interface IBudgetLineRepository : IGenericRepository<BudgetLine>
    {
    }
}

