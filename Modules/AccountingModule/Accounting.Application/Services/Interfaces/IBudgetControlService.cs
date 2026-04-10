using Accounting.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounting.Application.Services.Interfaces
{
    /// <summary>
    /// Checks whether a spending transaction respects active budget limits.
    /// Returns Result.Ok, Result.SuccessWithWarning or Result.Failure.
    /// </summary>
    public interface IBudgetControlService
    {
        /// <summary>
        /// Checks a single (accountId, costCenterId, amount) triple against the active budget.
        /// </summary>
        Task<Result> CheckBudgetAsync(int accountId, int? costCenterId, decimal amount, DateTime date);

        /// <summary>
        /// Checks a list of budget items (aggregated by accountId + costCenterId) in one call.
        /// Returns the first failure/warning encountered, or Ok if all pass.
        /// </summary>
        Task<Result> CheckBudgetListAsync(IEnumerable<BudgetCheckItem> items, DateTime date);
    }

    public class BudgetCheckItem
    {
        public int AccountId { get; set; }
        public int? CostCenterId { get; set; }
        public decimal Amount { get; set; }
    }
}
