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
    public class GetProfitByCostCenterQueryHandler : IRequestHandler<GetProfitByCostCenterQuery, Result<IEnumerable<ProfitByCostCenterDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProfitByCostCenterQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<ProfitByCostCenterDto>>> Handle(GetProfitByCostCenterQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.JournalEntryLines.Query()
                .AsNoTracking()
                .Where(l => l.JournalEntry.Status == JournalStatus.Posted);

            if (request.FromDate.HasValue)
                query = query.Where(l => l.JournalEntry.Date >= request.FromDate.Value);
            
            if (request.ToDate.HasValue)
                query = query.Where(l => l.JournalEntry.Date <= request.ToDate.Value);

            var data = await query
                .Where(l => l.Account.AccountType == AccountType.Revenue || l.Account.AccountType == AccountType.Expense)
                .GroupBy(l => new { l.CostCenterId, CostCenterName = l.CostCenter != null ? l.CostCenter.NameAr : null })
                .Select(g => new
                {
                    CostCenterId = g.Key.CostCenterId,
                    CostCenterName = g.Key.CostCenterName,
                    Revenue = g.Where(x => x.Account.AccountType == AccountType.Revenue).Sum(x => x.Credit - x.Debit),
                    Expenses = g.Where(x => x.Account.AccountType == AccountType.Expense).Sum(x => x.Debit - x.Credit)
                })
                .ToListAsync(cancellationToken);

            var result = data.Select(d => new ProfitByCostCenterDto
            {
                CostCenterId = d.CostCenterId,
                CostCenterName = d.CostCenterName,
                Revenue = d.Revenue,
                Expenses = d.Expenses,
                Profit = d.Revenue - d.Expenses
            }).OrderByDescending(x => x.Profit).ToList();

            return Result<IEnumerable<ProfitByCostCenterDto>>.Ok(result);
        }
    }
}
