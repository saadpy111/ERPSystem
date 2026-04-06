using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.CurrencyRates.Queries.GetCurrencyRatesList
{
    public class CurrencyRateDto
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
        public decimal? OfficialRate { get; set; }
        public string Source { get; set; } = null!;
    }

    public class GetCurrencyRatesQuery : IRequest<List<CurrencyRateDto>>
    {
        public int CurrencyId { get; set; }
    }

    public class GetCurrencyRatesQueryHandler : IRequestHandler<GetCurrencyRatesQuery, List<CurrencyRateDto>>
    {
        private readonly IAccountingDbContext _context;

        public GetCurrencyRatesQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<List<CurrencyRateDto>> Handle(GetCurrencyRatesQuery request, CancellationToken cancellationToken)
        {
            return await _context.CurrencyRates
                .AsNoTracking()
                .Where(r => r.CurrencyId == request.CurrencyId)
                .OrderByDescending(r => r.EffectiveDate)
                .Select(r => new CurrencyRateDto
                {
                    Id = r.Id,
                    CurrencyId = r.CurrencyId,
                    EffectiveDate = r.EffectiveDate,
                    BuyRate = r.BuyRate,
                    SellRate = r.SellRate,
                    OfficialRate = r.OfficialRate,
                    Source = r.Source
                })
                .ToListAsync(cancellationToken);
        }
    }
}
