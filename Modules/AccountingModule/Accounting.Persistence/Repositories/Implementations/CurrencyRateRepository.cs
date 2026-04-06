using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class CurrencyRateRepository : GenericRepository<CurrencyRate>, ICurrencyRateRepository
    {
        public CurrencyRateRepository(AccountingDbContext context) : base(context)
        {
        }

        public Task<bool> ExistsForDateAsync(int currencyId, DateTime date)
        {
            return _context.CurrencyRates.AnyAsync(cr => cr.CurrencyId == currencyId && cr.EffectiveDate.Date == date.Date);
        }

        public Task<CurrencyRate?> GetLatestRateAsync(int currencyId)
        {
            return _context.CurrencyRates
                .Where(cr => cr.CurrencyId == currencyId && cr.EffectiveDate <= DateTime.UtcNow)
                .OrderByDescending(cr => cr.EffectiveDate)
                .FirstOrDefaultAsync();
        }

        public Task<CurrencyRate?> GetRateByDateAsync(int currencyId, DateTime date)
        {
            return _context.CurrencyRates
                .Where(cr => cr.CurrencyId == currencyId && cr.EffectiveDate <= date)
                .OrderByDescending(cr => cr.EffectiveDate)
                .FirstOrDefaultAsync();
        }
    }
}
