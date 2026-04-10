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
                    return result;               // hard failure — stop immediately

                if (result.HasWarning)
                    warning = result;            // keep the first warning; keep checking
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

            var budget = budgetResult.Data!;

            // 2. Resolve the matching BudgetLine (exact → global fallback)
            var lineResult = ResolveBudgetLine(budget, accountId, costCenterId, date);
            if (!lineResult.Success)
                return lineResult;

            // 3. No BudgetLine found → allow transaction
            if (lineResult.Data == null)
                return Result.Ok();

            var line = lineResult.Data;

            // 4. Calculate actuals from posted journal entry lines
            decimal actual = await _unitOfWork.JournalEntries
                .GetActualAmountAsync(accountId, line.CostCenterId, line.StartDate, line.EndDate);

            decimal newAmount = actual + amount;

            // 5. Enforce decision
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

        /// <summary>
        /// Loads all Active budgets whose FiscalYear contains <paramref name="date"/>
        /// and enforces the rule that EXACTLY ONE Active budget exists per period.
        /// </summary>
        private async Task<Result<Budget>> ResolveBudgetAsync(DateTime date)
        {
            var activeBudgets = (await _unitOfWork.Budgets.GetActiveBudgetsForDateAsync(date)).ToList();

            if (activeBudgets.Count == 0)
                return Result<Budget>.Ok(null!, "No active budget found — transaction allowed.");

            if (activeBudgets.Count > 1)
                return Result<Budget>.Failure(
                    $"Multiple active budgets found for the fiscal year containing {date:yyyy-MM-dd}. " +
                    "Only one active budget is permitted per fiscal year.");

            return Result<Budget>.Ok(activeBudgets[0]);
        }

        // ─── BudgetLine Resolution ─────────────────────────────────────────────────

        /// <summary>
        /// Tries to find EXACTLY ONE matching BudgetLine.
        /// Priority: Exact (AccountId + CostCenterId) → Global (AccountId + null).
        /// <para>Returns null Data if no line exists (allow).</para>
        /// <para>Returns Failure if more than one line is found (data issue).</para>
        /// </summary>
        private Result<BudgetLine?> ResolveBudgetLine(
            Budget budget,
            int accountId,
            int? costCenterId,
            DateTime date)
        {
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
                    $"Multiple budget lines found for Account {accountId} / CostCenter {costCenterId}. " +
                    "This is a data integrity issue — please correct the budget configuration.");

            if (exactMatches.Count == 1)
                return Result<BudgetLine?>.Ok(exactMatches[0]);

            // --- Global fallback (only if a specific cost center was requested) ---
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
                        $"Multiple global budget lines found for Account {accountId} (null CostCenter). " +
                        "This is a data integrity issue — please correct the budget configuration.");

                if (globalMatches.Count == 1)
                    return Result<BudgetLine?>.Ok(globalMatches[0]);
            }

            // --- No line found → allow ---
            return Result<BudgetLine?>.Ok(null);
        }
    }
}
