using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Reports.DTOs;
using Accounting.Domain.Enums;
using MediatR;
using Accounting.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Reports.Queries.GetTrialBalance
{
    public class GetTrialBalanceQueryHandler : IRequestHandler<GetTrialBalanceQuery, Result<List<TrialBalanceDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTrialBalanceQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<TrialBalanceDto>>> Handle(GetTrialBalanceQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.JournalEntryLines.Query()
                .Where(l => l.JournalEntry.Status == JournalStatus.Posted);

            if (request.CostCenterId.HasValue)
            {
                query = query.Where(l => l.CostCenterId == request.CostCenterId.Value);
            }

            if (request.FromDate.HasValue)
            {
                query = query.Where(l => l.JournalEntry.Date >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                query = query.Where(l => l.JournalEntry.Date <= request.ToDate.Value);
            }

            var rawData = await query
                .GroupBy(l => new { l.AccountId, l.Account.Code, l.Account.NameAr, l.Account.NameEn, l.Account.AccountType })
                .Select(g => new
                {
                    AccountId = g.Key.AccountId,
                    AccountCode = g.Key.Code,
                    AccountName = g.Key.NameEn ?? g.Key.NameAr,
                    AccountType = g.Key.AccountType,
                    TotalDebit = g.Sum(x => x.Debit),
                    TotalCredit = g.Sum(x => x.Credit)
                })
                .ToListAsync(cancellationToken);

            var result = rawData
                .Where(x => x.TotalDebit != 0m || x.TotalCredit != 0m)
                .Select(x => 
                {
                    decimal balance = 0m;
                    
                    if (x.AccountType == AccountType.Asset || x.AccountType == AccountType.Expense)
                    {
                        balance = x.TotalDebit - x.TotalCredit;
                    }
                    else // Liability, Equity, Revenue
                    {
                        balance = x.TotalCredit - x.TotalDebit;
                    }

                    return new TrialBalanceDto
                    {
                        AccountId = x.AccountId,
                        AccountCode = x.AccountCode,
                        AccountName = x.AccountName,
                        Debit = x.TotalDebit,
                        Credit = x.TotalCredit,
                        Balance = balance,
                        CostCenterId = request.CostCenterId
                    };
                })
                .OrderBy(x => x.AccountCode)
                .ToList();

            return Result<List<TrialBalanceDto>>.Ok(result);
        }
    }
}
