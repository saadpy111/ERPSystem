using Accounting.Application.Common.Models;
using Accounting.Application.Reports.DTOs;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Reports.Queries
{
    public class GetBudgetVsActualReportQueryHandler : IRequestHandler<GetBudgetVsActualReportQuery, Result<Accounting.Application.Reports.Models.ReportResponse<BudgetVsActualItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBudgetVsActualReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Accounting.Application.Reports.Models.ReportResponse<BudgetVsActualItemDto>>> Handle(GetBudgetVsActualReportQuery request, CancellationToken cancellationToken)
        {
            var budgetLinesQuery = _unitOfWork.BudgetLines.Query()
                .AsNoTracking()
                .Include(bl => bl.Account)
                .Include(bl => bl.CostCenter)
                .Where(bl => bl.BudgetId == request.BudgetId);

            if (request.CostCenterId.HasValue)
            {
                budgetLinesQuery = budgetLinesQuery.Where(bl => bl.CostCenterId == request.CostCenterId.Value);
            }

            var budgetLines = await budgetLinesQuery.ToListAsync(cancellationToken);
            var items = new List<BudgetVsActualItemDto>();

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
                    .Select(x => new { x.Debit, x.Credit, AccountType = x.Account.AccountType })
                    .ToListAsync(cancellationToken);

                decimal actualAmount = rawData.Sum(x => x.AccountType == AccountType.Revenue ? (x.Credit - x.Debit) : (x.Debit - x.Credit));
                
                var usagePercentage = line.PlannedAmount == 0 ? 0 : (actualAmount / line.PlannedAmount) * 100;

                items.Add(new BudgetVsActualItemDto
                {
                    AccountId = line.AccountId,
                    AccountCode = line.Account.Code,
                    AccountName = line.Account.NameEn ?? line.Account.NameAr,
                    CostCenterId = line.CostCenterId,
                    CostCenterName = line.CostCenter?.NameAr,
                    PlannedAmount = line.PlannedAmount,
                    ActualAmount = actualAmount,
                    Variance = line.PlannedAmount - actualAmount,
                    UsagePercentage = usagePercentage
                });
            }

            var sortedItems = items.OrderBy(x => x.AccountCode).ToList();
            var response = new Accounting.Application.Reports.Models.ReportResponse<BudgetVsActualItemDto>
            {
                Items = sortedItems,
                Metadata = new Accounting.Application.Reports.Models.ReportMetadata
                {
                    ReportName = "Budget Vs Actual"
                }
            };
            
            response.Totals.Add("Total Planned", sortedItems.Sum(x => x.PlannedAmount));
            response.Totals.Add("Total Actual", sortedItems.Sum(x => x.ActualAmount));
            response.Totals.Add("Total Variance", sortedItems.Sum(x => x.Variance));

            return Result<Accounting.Application.Reports.Models.ReportResponse<BudgetVsActualItemDto>>.Ok(response);
        }
    }
}
