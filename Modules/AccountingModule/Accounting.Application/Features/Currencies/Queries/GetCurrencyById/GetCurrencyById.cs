using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Interfaces.Contexts;
using Accounting.Application.Features.Currencies.Queries.GetCurrenciesList;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Application.Features.Currencies.Queries.GetCurrencyById
{
    public class GetCurrencyByIdQuery : IRequest<CurrencyDto?>
    {
        public int Id { get; set; }
    }

    public class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, CurrencyDto?>
    {
        private readonly IAccountingDbContext _context;

        public GetCurrencyByIdQueryHandler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<CurrencyDto?> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
        {
            var currency = await _context.Currencies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (currency == null) return null;

            return new CurrencyDto
            {
                Id = currency.Id,
                Code = currency.Code,
                NameAr = currency.NameAr,
                NameEn = currency.NameEn,
                Symbol = currency.Symbol,
                DecimalPlaces = currency.DecimalPlaces,
                IsBaseCurrency = currency.IsBaseCurrency,
                IsActive = currency.IsActive
            };
        }
    }
}
