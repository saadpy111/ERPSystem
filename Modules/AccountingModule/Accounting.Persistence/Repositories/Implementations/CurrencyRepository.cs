using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using Accounting.Persistence.Common;
using Accounting.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Accounting.Persistence.Repositories.Implementations
{
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(AccountingDbContext context) : base(context)
        {
        }

        public Task<Currency?> GetBaseCurrencyAsync()
        {
            return _context.Currencies.FirstOrDefaultAsync(c => c.IsBaseCurrency && c.IsActive);
        }

        public async Task<bool> IsCodeUniqueAsync(string code)
        {
            return !await _context.Currencies.AnyAsync(c => c.Code == code);
        }

        public Task<Currency?> GetWithRatesAsync(int id)
        {
            return _context.Currencies.Include(c => c.CurrencyRates).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
