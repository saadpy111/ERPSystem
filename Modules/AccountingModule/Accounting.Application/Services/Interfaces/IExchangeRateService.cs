using System;
using System.Threading.Tasks;

namespace Accounting.Application.Services.Interfaces
{
    public interface IExchangeRateService
    {
        Task<decimal> GetRateAsync(int currencyId, DateTime date);
        Task<decimal> ConvertAsync(decimal amount, int fromCurrencyId, int toCurrencyId, DateTime date);
    }
}
