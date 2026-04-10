using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Reports.DTOs;
using Accounting.Domain.Enums;
using MediatR;
using Accounting.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Reports.Queries.GetBalanceSheet
{
    public class GetBalanceSheetQueryHandler : IRequestHandler<GetBalanceSheetQuery, Result<BalanceSheetDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBalanceSheetQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BalanceSheetDto>> Handle(GetBalanceSheetQuery request, CancellationToken cancellationToken)
        {
            // Note: Cost Center filtering may produce unbalanced results because Balance Sheet is cumulative across all transactions.
            
            var query = _unitOfWork.JournalEntryLines.Query()
                .Where(l => l.JournalEntry.Status == JournalStatus.Posted && l.JournalEntry.Date <= request.AsOfDate);

            if (request.CostCenterId.HasValue)
            {
                query = query.Where(l => l.CostCenterId == request.CostCenterId.Value);
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

            var assets = new List<BalanceSheetItemDto>();
            var liabilities = new List<BalanceSheetItemDto>();
            var equity = new List<BalanceSheetItemDto>();

            decimal netIncome = 0;

            foreach (var item in rawData)
            {
                if (item.AccountType == AccountType.Asset)
                {
                    decimal balance = item.TotalDebit - item.TotalCredit;
                    if (balance != 0)
                    {
                        assets.Add(new BalanceSheetItemDto
                        {
                            AccountId = item.AccountId,
                            AccountCode = item.AccountCode,
                            AccountName = item.AccountName,
                            Amount = balance,
                            CostCenterId = request.CostCenterId
                        });
                    }
                }
                else if (item.AccountType == AccountType.Liability)
                {
                    decimal balance = item.TotalCredit - item.TotalDebit;
                    if (balance != 0)
                    {
                        liabilities.Add(new BalanceSheetItemDto
                        {
                            AccountId = item.AccountId,
                            AccountCode = item.AccountCode,
                            AccountName = item.AccountName,
                            Amount = balance,
                            CostCenterId = request.CostCenterId
                        });
                    }
                }
                else if (item.AccountType == AccountType.Equity)
                {
                    decimal balance = item.TotalCredit - item.TotalDebit;
                    if (balance != 0)
                    {
                        equity.Add(new BalanceSheetItemDto
                        {
                            AccountId = item.AccountId,
                            AccountCode = item.AccountCode,
                            AccountName = item.AccountName,
                            Amount = balance,
                            CostCenterId = request.CostCenterId
                        });
                    }
                }
                else if (item.AccountType == AccountType.Revenue)
                {
                    netIncome += (item.TotalCredit - item.TotalDebit);
                }
                else if (item.AccountType == AccountType.Expense)
                {
                    netIncome -= (item.TotalDebit - item.TotalCredit);
                }
            }

            if (netIncome != 0)
            {
                equity.Add(new BalanceSheetItemDto
                {
                    AccountId = 0,
                    AccountCode = "-",
                    AccountName = "Calculated Net Income",
                    Amount = netIncome,
                    CostCenterId = request.CostCenterId
                });
            }

            assets = assets.OrderBy(a => a.AccountCode).ToList();
            liabilities = liabilities.OrderBy(l => l.AccountCode).ToList();
            equity = equity.OrderBy(e => e.AccountCode).ToList();

            decimal totalAssets = assets.Sum(a => a.Amount);
            decimal totalLiabilities = liabilities.Sum(l => l.Amount);
            decimal totalEquity = equity.Sum(e => e.Amount);

            // Validation logic: Skip if filtered by Cost Center
            bool isBalanced = Math.Abs(totalAssets - (totalLiabilities + totalEquity)) <= 0.01m;
            
            if (!request.CostCenterId.HasValue && !isBalanced)
            {
                return Result<BalanceSheetDto>.Failure($"Balance Sheet mismatch: Total Assets ({totalAssets}) != Total Liabilities ({totalLiabilities}) + Total Equity ({totalEquity}).");
            }

            return Result<BalanceSheetDto>.Ok(new BalanceSheetDto
            {
                Assets = assets,
                Liabilities = liabilities,
                Equity = equity,
                TotalAssets = totalAssets,
                TotalLiabilities = totalLiabilities,
                TotalEquity = totalEquity,
                IsBalanced = isBalanced,
                AsOfDate = request.AsOfDate
            });
        }
    }
}
