using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Currencies.Queries.GetCurrenciesList
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public int DecimalPlaces { get; set; }
        public bool IsBaseCurrency { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetCurrenciesQuery : IRequest<Result<List<CurrencyDto>>>
    {
    }

    public class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, Result<List<CurrencyDto>>>
    {
        private readonly IAccountingDbContext _context;

        public GetCurrenciesQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<CurrencyDto>>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Currencies
                .AsNoTracking()
                .Select(c => new CurrencyDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    NameAr = c.NameAr,
                    NameEn = c.NameEn,
                    Symbol = c.Symbol,
                    DecimalPlaces = c.DecimalPlaces,
                    IsBaseCurrency = c.IsBaseCurrency,
                    IsActive = c.IsActive
                })
                .ToListAsync(cancellationToken);
        }
    }
}
