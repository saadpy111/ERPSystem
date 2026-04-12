using System;
using Accounting.Application.Common.Models;
using Accounting.Application.Dashboard.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Dashboard.Queries
{
    public class GetBudgetUsageQueryHandler : IRequestHandler<GetBudgetUsageQuery, Result<IEnumerable<BudgetUsageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBudgetUsageQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<BudgetUsageDto>>> Handle(GetBudgetUsageQuery request, CancellationToken cancellationToken)
        {
            var budgetLines = await _unitOfWork.BudgetLines.Query()
                .AsNoTracking()
                .Include(bl => bl.Budget)
                .Where(bl => bl.BudgetId == request.BudgetId)
                .ToListAsync(cancellationToken);

            var result = new List<BudgetUsageDto>();

            foreach (var line in budgetLines)
            {
                var actualsQuery = _unitOfWork.JournalEntryLines.Query()
                    .AsNoTracking()
                    .Where(l => l.AccountId == line.AccountId
                             && l.JournalEntry.Status == JournalStatus.Posted
                             && l.JournalEntry.Date >= line.StartDate
                             && l.JournalEntry.Date <= line.EndDate);

                if (line.CostCenterId.HasValue)
                {
                    actualsQuery = actualsQuery.Where(l => l.CostCenterId == line.CostCenterId.Value);
                }

                var rawData = await actualsQuery
                    .Select(x => new { x.Debit, x.Credit })
                    .ToListAsync(cancellationToken);

                var actualAmount = rawData.Sum(x => x.Debit - x.Credit); // Assuming budgets are for expenses primarily, adjust if needed
                
                var usagePercentage = line.PlannedAmount == 0 ? 0 : (actualAmount / line.PlannedAmount) * 100;
                
                string status = usagePercentage > 100 ? "Exceeded" : (usagePercentage >= 80 ? "Warning" : "Normal");

                result.Add(new BudgetUsageDto
                {
                    BudgetId = line.BudgetId,
                    BudgetName = line.Budget.Name,
                    CostCenterId = line.CostCenterId,
                    CostCenterName = line.CostCenter?.NameAr,
                    PlannedAmount = line.PlannedAmount,
                    ActualAmount = actualAmount,
                    UsagePercentage = usagePercentage,
                    Status = status
                });
            }

            return Result<IEnumerable<BudgetUsageDto>>.Ok(result.OrderByDescending(x => x.UsagePercentage).ToList());
        }
    }
}
