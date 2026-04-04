using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Reports.DTOs;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Reports.Queries.GetBalanceSheet
{
    public class GetBalanceSheetQueryHandler : IRequestHandler<GetBalanceSheetQuery, BalanceSheetDto>
    {
        private readonly IAccountingDbContext _context;

        public GetBalanceSheetQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<BalanceSheetDto> Handle(GetBalanceSheetQuery request, CancellationToken cancellationToken)
        {
            // 3. DATA SOURCE: Only Posted entries, Date <= AsOfDate
            // 11. PERFORMANCE: AsNoTracking(), Select, no Include, group in DB directly
            var rawData = await _context.JournalEntryLines
                .AsNoTracking()
                .Where(l => l.JournalEntry.Status == JournalStatus.Posted && l.JournalEntry.Date <= request.AsOfDate)
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

            // 5. CALCULATE BALANCE & 6. CLASSIFICATION & 7. FILTER ZERO ACCOUNTS
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
                            Amount = balance
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
                            Amount = balance
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
                            Amount = balance
                        });
                    }
                }
                else if (item.AccountType == AccountType.Revenue)
                {
                    // Revenue adds to Retained Earnings (Equity)
                    netIncome += (item.TotalCredit - item.TotalDebit);
                }
                else if (item.AccountType == AccountType.Expense)
                {
                    // Expense reduces Retained Earnings (Equity)
                    netIncome -= (item.TotalDebit - item.TotalCredit);
                }
            }

            // In order to make Assets = Liabilities + Equity balance, we must include the current year's
            // Net Income into the Equity section (Calculated Retained Earnings).
            if (netIncome != 0)
            {
                equity.Add(new BalanceSheetItemDto
                {
                    AccountId = 0, // Virtual Account
                    AccountCode = "-",
                    AccountName = "Calculated Net Income",
                    Amount = netIncome
                });
            }

            // 12. ORDERING
            assets = assets.OrderBy(a => a.AccountCode).ToList();
            liabilities = liabilities.OrderBy(l => l.AccountCode).ToList();
            equity = equity.OrderBy(e => e.AccountCode).ToList();

            // 9. TOTALS
            decimal totalAssets = assets.Sum(a => a.Amount);
            decimal totalLiabilities = liabilities.Sum(l => l.Amount);
            decimal totalEquity = equity.Sum(e => e.Amount);

            // 10. VALIDATION
            if (Math.Abs(totalAssets - (totalLiabilities + totalEquity)) > 0.01m)
            {
                // This will fail the operation directly, ensuring accuracy.
                throw new BusinessException($"Balance Sheet mismatch: Total Assets ({totalAssets}) != Total Liabilities ({totalLiabilities}) + Total Equity ({totalEquity}).");
            }

            // 8. STRUCTURE OUTPUT
            return new BalanceSheetDto
            {
                Assets = assets,
                Liabilities = liabilities,
                Equity = equity,
                TotalAssets = totalAssets,
                TotalLiabilities = totalLiabilities,
                TotalEquity = totalEquity,
                IsBalanced = true,
                AsOfDate = request.AsOfDate
            };
        }
    }
}
