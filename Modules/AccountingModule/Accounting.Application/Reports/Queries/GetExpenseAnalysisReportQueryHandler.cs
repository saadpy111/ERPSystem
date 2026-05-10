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
    public class GetExpenseAnalysisReportQueryHandler : IRequestHandler<GetExpenseAnalysisReportQuery, Result<Accounting.Application.Reports.Models.ReportResponse<ExpenseAnalysisItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetExpenseAnalysisReportQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Accounting.Application.Reports.Models.ReportResponse<ExpenseAnalysisItemDto>>> Handle(GetExpenseAnalysisReportQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.JournalEntryLines.Query()
                .AsNoTracking()
                .Include(l => l.Account)
                .Include(l => l.CostCenter)
                .Where(l => l.Account.AccountType == AccountType.Expense 
                         && l.JournalEntry.Status == JournalStatus.Posted
                         && l.JournalEntry.Date >= request.FromDate
                         && l.JournalEntry.Date <= request.ToDate);

            if (request.CostCenterId.HasValue)
            {
                query = query.Where(l => l.CostCenterId == request.CostCenterId.Value);
            }

            var rawData = await query.ToListAsync(cancellationToken);

            var groupedData = rawData
                .GroupBy(l => new { l.AccountId, l.Account.Code, Name = l.Account.NameEn ?? l.Account.NameAr, l.CostCenterId, CostCenterName = l.CostCenter?.NameAr })
                .Select(g => new
                {
                    AccountId = g.Key.AccountId,
                    AccountCode = g.Key.Code,
                    AccountName = g.Key.Name,
                    CostCenterId = g.Key.CostCenterId,
                    CostCenterName = g.Key.CostCenterName,
                    Amount = g.Sum(x => x.Debit - x.Credit)
                })
                .Where(x => x.Amount != 0)
                .ToList();

            var totalExpense = groupedData.Sum(x => x.Amount);

            var items = groupedData.Select(x => new ExpenseAnalysisItemDto
            {
                AccountId = x.AccountId,
                AccountCode = x.AccountCode,
                AccountName = x.AccountName,
                CostCenterId = x.CostCenterId,
                CostCenterName = x.CostCenterName,
                Amount = x.Amount,
                PercentageOfTotal = totalExpense == 0 ? 0 : (x.Amount / totalExpense) * 100
            }).OrderByDescending(x => x.Amount).ToList();

            var response = new Accounting.Application.Reports.Models.ReportResponse<ExpenseAnalysisItemDto>
            {
                Items = items,
                Metadata = new Accounting.Application.Reports.Models.ReportMetadata
                {
                    ReportName = "Expense Analysis",
                    FromDate = request.FromDate,
                    ToDate = request.ToDate
                }
            };
            
            response.Totals.Add("Total Expense", totalExpense);

            return Result<Accounting.Application.Reports.Models.ReportResponse<ExpenseAnalysisItemDto>>.Ok(response);
        }
    }
}
