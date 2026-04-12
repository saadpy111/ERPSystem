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
    public class GetTopExpensesQueryHandler : IRequestHandler<GetTopExpensesQuery, Result<IEnumerable<TopExpenseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTopExpensesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<TopExpenseDto>>> Handle(GetTopExpensesQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.JournalEntryLines.Query()
                .AsNoTracking()
                .Where(l => l.Account.AccountType == AccountType.Expense && l.JournalEntry.Status == JournalStatus.Posted)
                .GroupBy(l => new { l.AccountId, l.Account.Code, Name = l.Account.NameEn ?? l.Account.NameAr })
                .Select(g => new
                {
                    AccountId = g.Key.AccountId,
                    AccountCode = g.Key.Code,
                    AccountName = g.Key.Name,
                    TotalExpense = g.Sum(x => x.Debit - x.Credit)
                })
                .OrderByDescending(x => x.TotalExpense)
                .Take(request.NumberOfRecords)
                .ToListAsync(cancellationToken);

            var result = data.Select(x => new TopExpenseDto
            {
                AccountId = x.AccountId,
                AccountCode = x.AccountCode,
                AccountName = x.AccountName,
                TotalExpense = x.TotalExpense
            });

            return Result<IEnumerable<TopExpenseDto>>.Ok(result);
        }
    }
}
