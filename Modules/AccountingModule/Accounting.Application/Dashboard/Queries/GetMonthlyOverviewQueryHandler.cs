using System;
using System.Globalization;
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
    public class GetMonthlyOverviewQueryHandler : IRequestHandler<GetMonthlyOverviewQuery, Result<IEnumerable<MonthlyOverviewDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMonthlyOverviewQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<MonthlyOverviewDto>>> Handle(GetMonthlyOverviewQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.JournalEntryLines.Query()
                .AsNoTracking()
                .Where(l => l.JournalEntry.Status == JournalStatus.Posted && l.JournalEntry.Date.Year == request.Year)
                .Where(l => l.Account.AccountType == AccountType.Revenue || l.Account.AccountType == AccountType.Expense)
                .GroupBy(l => new { l.JournalEntry.Date.Month })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Revenue = g.Where(x => x.Account.AccountType == AccountType.Revenue).Sum(x => x.Credit - x.Debit),
                    Expenses = g.Where(x => x.Account.AccountType == AccountType.Expense).Sum(x => x.Debit - x.Credit)
                })
                .ToListAsync(cancellationToken);

            var result = Enumerable.Range(1, 12).Select(month => 
            {
                var monthData = data.FirstOrDefault(d => d.Month == month);
                return new MonthlyOverviewDto
                {
                    Year = request.Year,
                    Month = month,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    Revenue = monthData?.Revenue ?? 0,
                    Expenses = monthData?.Expenses ?? 0,
                    Profit = (monthData?.Revenue ?? 0) - (monthData?.Expenses ?? 0)
                };
            }).ToList();

            return Result<IEnumerable<MonthlyOverviewDto>>.Ok(result);
        }
    }
}
