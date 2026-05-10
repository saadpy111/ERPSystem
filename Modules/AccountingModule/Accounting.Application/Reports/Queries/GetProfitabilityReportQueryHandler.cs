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
    public class GetProfitabilityReportQueryHandler : IRequestHandler<GetProfitabilityReportQuery, Result<Accounting.Application.Reports.Models.ReportResponse<ProfitabilityItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProfitabilityReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Accounting.Application.Reports.Models.ReportResponse<ProfitabilityItemDto>>> Handle(GetProfitabilityReportQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.JournalEntryLines.Query()
                .AsNoTracking()
                .Include(l => l.Account)
                .Include(l => l.CostCenter)
                .Where(l => (l.Account.AccountType == AccountType.Revenue || l.Account.AccountType == AccountType.Expense)
                         && l.JournalEntry.Status == JournalStatus.Posted
                         && l.JournalEntry.Date >= request.FromDate
                         && l.JournalEntry.Date <= request.ToDate);

            if (request.CostCenterId.HasValue)
            {
                query = query.Where(l => l.CostCenterId == request.CostCenterId.Value);
            }

            var rawData = await query.ToListAsync(cancellationToken);

            var groupedData = rawData
                .GroupBy(l => new { l.CostCenterId, CostCenterName = l.CostCenter?.NameAr })
                .Select(g => new
                {
                    CostCenterId = g.Key.CostCenterId,
                    CostCenterName = g.Key.CostCenterName,
                    Revenue = g.Where(x => x.Account.AccountType == AccountType.Revenue).Sum(x => x.Credit - x.Debit),
                    Expenses = g.Where(x => x.Account.AccountType == AccountType.Expense).Sum(x => x.Debit - x.Credit)
                })
                .ToList();

            var items = groupedData.Select(x => 
            {
                var profit = x.Revenue - x.Expenses;
                return new ProfitabilityItemDto
                {
                    CostCenterId = x.CostCenterId,
                    CostCenterName = x.CostCenterName,
                    Revenue = x.Revenue,
                    Expenses = x.Expenses,
                    Profit = profit,
                    ProfitMargin = x.Revenue == 0 ? 0 : (profit / x.Revenue) * 100
                };
            }).OrderByDescending(x => x.Profit).ToList();

            var totalRevenue = items.Sum(x => x.Revenue);
            var totalExpenses = items.Sum(x => x.Expenses);

            var response = new Accounting.Application.Reports.Models.ReportResponse<ProfitabilityItemDto>
            {
                Items = items,
                Metadata = new Accounting.Application.Reports.Models.ReportMetadata
                {
                    ReportName = "Profitability",
                    FromDate = request.FromDate,
                    ToDate = request.ToDate
                }
            };
            
            response.Totals.Add("Total Revenue", totalRevenue);
            response.Totals.Add("Total Expenses", totalExpenses);
            response.Totals.Add("Total Profit", totalRevenue - totalExpenses);

            return Result<Accounting.Application.Reports.Models.ReportResponse<ProfitabilityItemDto>>.Ok(response);
        }
    }
}
