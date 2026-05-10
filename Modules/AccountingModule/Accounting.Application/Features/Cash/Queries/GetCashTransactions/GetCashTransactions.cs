using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Cash.Queries.GetCashTransactions
{
    public class CashTransactionDto
    {
        public int Id { get; set; }
        public CashTransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public string? Reference { get; set; }
        public string? PartnerName { get; set; }
        public string CurrencyCode { get; set; } = null!;
    }

    public class GetCashTransactionsQuery : IRequest<Result<PagedResult<CashTransactionDto>>>
    {
        public CashTransactionType? Type { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? CashAccountId { get; set; }
        public int? PartnerId { get; set; }
        public int? CurrencyId { get; set; }
        public string? Search { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetCashTransactionsQueryHandler : IRequestHandler<GetCashTransactionsQuery, Result<PagedResult<CashTransactionDto>>>
    {
        private readonly IAccountingDbContext _context;

        public GetCashTransactionsQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<PagedResult<CashTransactionDto>>> Handle(GetCashTransactionsQuery request, CancellationToken cancellationToken)
        {
            var q = _context.CashTransactions.AsNoTracking();

            if (request.CashAccountId.HasValue)
                q = q.Where(t => t.CashAccountId == request.CashAccountId.Value);
                
            if (request.Type.HasValue)
                q = q.Where(t => t.Type == request.Type.Value);
                
            if (request.DateFrom.HasValue)
                q = q.Where(t => t.Date >= request.DateFrom.Value);
                
            if (request.DateTo.HasValue)
                q = q.Where(t => t.Date <= request.DateTo.Value);
                
            if (request.PartnerId.HasValue)
                q = q.Where(t => t.PartnerId == request.PartnerId.Value);
                
            if (request.CurrencyId.HasValue)
                q = q.Where(t => t.CurrencyId == request.CurrencyId.Value);
                
            if (request.MinAmount.HasValue)
                q = q.Where(t => t.Amount >= request.MinAmount.Value);
                
            if (request.MaxAmount.HasValue)
                q = q.Where(t => t.Amount <= request.MaxAmount.Value);
                
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                q = q.Where(t => t.Description.ToLower().Contains(search) || 
                                (t.Reference != null && t.Reference.ToLower().Contains(search)));
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new CashTransactionDto
                {
                    Id = t.Id,
                    Type = t.Type,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                    Reference = t.Reference,
                    PartnerName = t.Partner != null ? t.Partner.NameEn : null,
                    CurrencyCode = t.Currency.Code
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<CashTransactionDto>
            {
                Data = items,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
