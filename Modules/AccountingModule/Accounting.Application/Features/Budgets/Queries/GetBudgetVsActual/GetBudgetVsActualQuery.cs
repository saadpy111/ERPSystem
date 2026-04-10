using Accounting.Application.Common.Models;
using Accounting.Application.Features.Budgets.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Budgets.Queries.GetBudgetVsActual
{
    public class GetBudgetVsActualQuery : IRequest<Result<List<BudgetVsActualDto>>>
    {
        public int BudgetId { get; set; }
    }

    public class GetBudgetVsActualQueryHandler : IRequestHandler<GetBudgetVsActualQuery, Result<List<BudgetVsActualDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBudgetVsActualQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<BudgetVsActualDto>>> Handle(GetBudgetVsActualQuery request, CancellationToken cancellationToken)
        {
            var budgetLines = await _unitOfWork.BudgetLines.Query()
                .AsNoTracking()
                .Include(l => l.Account)
                .Include(l => l.CostCenter)
                .Where(l => l.BudgetId == request.BudgetId)
                .ToListAsync(cancellationToken);

            if (!budgetLines.Any())
            {
                // Check if budget exists at least
                var budgetExists = await _unitOfWork.Budgets.Query().AnyAsync(b => b.Id == request.BudgetId, cancellationToken);
                if (!budgetExists) return Result<List<BudgetVsActualDto>>.Failure("Budget not found.");
                return Result<List<BudgetVsActualDto>>.Ok(new List<BudgetVsActualDto>());
            }

            var results = new List<BudgetVsActualDto>();

            foreach (var line in budgetLines)
            {
                var actualQuery = _unitOfWork.JournalEntryLines.Query()
                    .Where(j => j.AccountId == line.AccountId && 
                                j.JournalEntry.Status == JournalStatus.Posted && 
                                j.JournalEntry.Date >= line.StartDate && 
                                j.JournalEntry.Date <= line.EndDate);

                if (line.CostCenterId.HasValue)
                {
                    actualQuery = actualQuery.Where(j => j.CostCenterId == line.CostCenterId.Value);
                }
                // If CostCenterId is null, it includes all cost centers (no filter added)

                var actualAmount = await actualQuery
                    .SumAsync(j => j.Debit - j.Credit, cancellationToken);

                results.Add(new BudgetVsActualDto
                {
                    AccountId = line.AccountId,
                    AccountName = line.Account.NameEn ?? line.Account.NameAr,
                    CostCenterId = line.CostCenterId,
                    CostCenterName = line.CostCenter?.NameEn ?? line.CostCenter?.NameAr,
                    PlannedAmount = line.PlannedAmount,
                    ActualAmount = actualAmount,
                    Variance = actualAmount - line.PlannedAmount
                });
            }

            return Result<List<BudgetVsActualDto>>.Ok(results);
        }
    }
}
