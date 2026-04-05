using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Cash.Queries.GetCashTransactions
{
    public class CashTransactionDto
    {
        public int Id { get; set; }
        public CashTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
    }

    public class GetCashTransactionsQuery : IRequest<List<CashTransactionDto>>
    {
        public int? CashAccountId { get; set; }
    }

    public class GetCashTransactionsQueryHandler : IRequestHandler<GetCashTransactionsQuery, List<CashTransactionDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetCashTransactionsQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<List<CashTransactionDto>> Handle(GetCashTransactionsQuery request, CancellationToken cancellationToken)
        {
            var q = _context.CashTransactions.AsNoTracking();

            if (request.CashAccountId.HasValue)
            {
                q = q.Where(t => t.CashAccountId == request.CashAccountId.Value);
            }

            q = q.OrderByDescending(t => t.Date);

            return await q
                .Select(t => new CashTransactionDto
                {
                    Id = t.Id,
                    Type = t.Type,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description
                })
                .ToListAsync(cancellationToken);
        }
    }
}
