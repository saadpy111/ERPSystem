using Accounting.Application.Common.Models;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Services.Interfaces;
using Accounting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Application.Services.Implementations
{
    /// <summary>
    /// Core budget enforcement layer.
    /// Applies ONLY to Debit (expense) amounts; credit entries are ignored.
    /// </summary>
    public class BudgetControlService : IBudgetControlService
    {
        private readonly IUnitOfWork _unitOfWork;

    public BudgetControlService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ─── Public API ────────────────────────────────────────────────────────────

        public async Task<Result> CheckBudgetAsync(
            int accountId,
            int? costCenterId,
            decimal amount,
            DateTime date)
        {
            return await CheckSingleAsync(accountId, costCenterId, amount, date);
        }

        public async Task<Result> CheckBudgetListAsync(
            IEnumerable<BudgetCheckItem> items,
            DateTime date)
        {
            Result? warning = null;

            foreach (var item in items)
            {
                var result = await CheckSingleAsync(item.AccountId, item.CostCenterId, item.Amount, date);

                if (!result.Success)
                    return result;

                if (result.HasWarning)
                    warning = result;
            }

            return warning ?? Result.Ok();
        }

        // ─── Core Logic ────────────────────────────────────────────────────────────

        private async Task<Result> CheckSingleAsync(
            int accountId,
            int? costCenterId,
            decimal amount,
            DateTime date)
        {
            // 1. Resolve the active budget for this date
            var budgetResult = await ResolveBudgetAsync(date);
            if (!budgetResult.Success)
                return budgetResult;

            var budget = budgetResult.Data;

            //  FIX: No budget → allow
            if (budget == null)
                return Result.Ok();

            // 2. Resolve the matching BudgetLine
            var lineResult = ResolveBudgetLine(budget, accountId, costCenterId, date);
            if (!lineResult.Success)
                return lineResult;

            // 3. No BudgetLine → allow
            if (lineResult.Data == null)
                return Result.Ok();

            var line = lineResult.Data;

            // 4. Calculate actuals
            decimal actual = await _unitOfWork.JournalEntries
                .GetActualAmountAsync(accountId, line.CostCenterId, line.StartDate, line.EndDate);

            decimal newAmount = actual + amount;

            // 5. Enforce
            if (newAmount > line.PlannedAmount)
            {
                string msg = $"Budget exceeded for Account {accountId}" +
                             (line.CostCenterId.HasValue ? $" / CostCenter {line.CostCenterId}" : " (global)") +
                             $". Planned: {line.PlannedAmount:N2}, Actual: {actual:N2}, Requested: {amount:N2}.";

                return budget.EnforceBudgetControl
                    ? Result.Failure(msg)
                    : Result.SuccessWithWarning(msg);
            }

            return Result.Ok();
        }

        // ─── Budget Selection ──────────────────────────────────────────────────────

        private async Task<Result<Budget>> ResolveBudgetAsync(DateTime date)
        {
            var activeBudgets = (await _unitOfWork.Budgets.GetActiveBudgetsForDateAsync(date)).ToList();

            if (activeBudgets.Count == 0)
                return Result<Budget>.Ok(null!, "No active budget found — transaction allowed.");

            if (activeBudgets.Count > 1)
                return Result<Budget>.Failure(
                    $"Multiple active budgets found for {date:yyyy-MM-dd}. Only one is allowed.");

            return Result<Budget>.Ok(activeBudgets[0]);
        }

        // ─── BudgetLine Resolution ─────────────────────────────────────────────────

        private Result<BudgetLine?> ResolveBudgetLine(
            Budget budget,
            int accountId,
            int? costCenterId,
            DateTime date)
        {
            //  FIX: Null safety
            if (budget == null || budget.Lines == null || !budget.Lines.Any())
                return Result<BudgetLine?>.Ok(null);

            // --- Exact match ---
            var exactMatches = budget.Lines
                .Where(l =>
                    l.AccountId == accountId &&
                    l.CostCenterId == costCenterId &&
                    l.StartDate <= date &&
                    l.EndDate >= date)
                .ToList();

            if (exactMatches.Count > 1)
                return Result<BudgetLine?>.Failure(
                    $"Multiple budget lines for Account {accountId} / CostCenter {costCenterId}.");

            if (exactMatches.Count == 1)
                return Result<BudgetLine?>.Ok(exactMatches[0]);

            // --- Global fallback ---
            if (costCenterId.HasValue)
            {
                var globalMatches = budget.Lines
                    .Where(l =>
                        l.AccountId == accountId &&
                        l.CostCenterId == null &&
                        l.StartDate <= date &&
                        l.EndDate >= date)
                    .ToList();

                if (globalMatches.Count > 1)
                    return Result<BudgetLine?>.Failure(
                        $"Multiple global budget lines for Account {accountId}.");

                if (globalMatches.Count == 1)
                    return Result<BudgetLine?>.Ok(globalMatches[0]);
            }

            return Result<BudgetLine?>.Ok(null);
        }
    }

}
