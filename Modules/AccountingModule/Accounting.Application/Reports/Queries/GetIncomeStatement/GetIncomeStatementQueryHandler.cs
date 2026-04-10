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

namespace Accounting.Application.Reports.Queries.GetIncomeStatement
{
    public class GetIncomeStatementQueryHandler : IRequestHandler<GetIncomeStatementQuery, Result<IncomeStatementDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetIncomeStatementQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IncomeStatementDto>> Handle(GetIncomeStatementQuery request, CancellationToken cancellationToken)
        {
            if (request.FromDate > request.ToDate)
            {
                return Result<IncomeStatementDto>.Failure("FromDate cannot be later than ToDate.");
            }

            var query = _unitOfWork.JournalEntryLines.Query()
                .Where(l => l.JournalEntry.Status == JournalStatus.Posted 
                         && l.JournalEntry.Date >= request.FromDate 
                         && l.JournalEntry.Date <= request.ToDate);

            if (request.CostCenterId.HasValue)
            {
                query = query.Where(l => l.CostCenterId == request.CostCenterId.Value);
            }

            var resultsQuery = query
                .GroupBy(l => new 
                { 
                    l.AccountId, 
                    l.Account.Code, 
                    l.Account.NameAr, 
                    l.Account.NameEn, 
                    l.Account.AccountType,
                    CostCenterId = request.GroupByCostCenter ? l.CostCenterId : (int?)null,
                    CostCenterName = request.GroupByCostCenter ? (l.CostCenter != null ? l.CostCenter.NameAr : null) : null
                })
                .Select(g => new
                {
                    AccountId = g.Key.AccountId,
                    AccountCode = g.Key.Code,
                    AccountName = g.Key.NameEn ?? g.Key.NameAr,
                    AccountType = g.Key.AccountType,
                    CostCenterId = g.Key.CostCenterId,
                    CostCenterName = g.Key.CostCenterName,
                    TotalDebit = g.Sum(x => x.Debit),
                    TotalCredit = g.Sum(x => x.Credit)
                });

            var rawData = await resultsQuery.ToListAsync(cancellationToken);

            var revenues = new List<IncomeStatementItemDto>();
            var expenses = new List<IncomeStatementItemDto>();

            foreach (var item in rawData)
            {
                if (item.AccountType == AccountType.Revenue)
                {
                    decimal amount = item.TotalCredit - item.TotalDebit;
                    if (amount != 0)
                    {
                        revenues.Add(new IncomeStatementItemDto
                        {
                            AccountId = item.AccountId,
                            AccountCode = item.AccountCode,
                            AccountName = item.AccountName,
                            Amount = amount,
                            CostCenterId = item.CostCenterId,
                            CostCenterName = item.CostCenterName
                        });
                    }
                }
                else if (item.AccountType == AccountType.Expense)
                {
                    decimal amount = item.TotalDebit - item.TotalCredit;
                    if (amount != 0)
                    {
                        expenses.Add(new IncomeStatementItemDto
                        {
                            AccountId = item.AccountId,
                            AccountCode = item.AccountCode,
                            AccountName = item.AccountName,
                            Amount = amount,
                            CostCenterId = item.CostCenterId,
                            CostCenterName = item.CostCenterName
                        });
                    }
                }
            }

            revenues = revenues.OrderBy(r => r.AccountCode).ToList();
            expenses = expenses.OrderBy(e => e.AccountCode).ToList();

            decimal totalRevenue = revenues.Sum(r => r.Amount);
            decimal totalExpenses = expenses.Sum(e => e.Amount);
            decimal netProfit = totalRevenue - totalExpenses;

            return Result<IncomeStatementDto>.Ok(new IncomeStatementDto
            {
                Revenues = revenues,
                Expenses = expenses,
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses,
                NetProfit = netProfit,
                FromDate = request.FromDate,
                ToDate = request.ToDate
            });
        }
    }
}
