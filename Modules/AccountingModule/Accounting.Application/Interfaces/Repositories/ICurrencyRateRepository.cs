using Accounting.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Accounting.Application.Interfaces.Repositories
{
    public interface ICurrencyRateRepository : IGenericRepository<CurrencyRate>
    {
        Task<bool> ExistsForDateAsync(int currencyId, DateTime date);
        Task<CurrencyRate?> GetLatestRateAsync(int currencyId);
        Task<CurrencyRate?> GetRateByDateAsync(int currencyId, DateTime date);
    }
}
