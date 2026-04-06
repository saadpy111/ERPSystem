using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Application.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Accounting.Application.Services.Implementations
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExchangeRateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> GetRateAsync(int currencyId, DateTime date)
        {
            var currency = await _unitOfWork.Currencies.GetByIdAsync(currencyId);
            if (currency == null)
                throw new BusinessException($"Invalid currency ID: {currencyId}");

            if (currency.IsBaseCurrency)
                return 1m;

            var rate = await _unitOfWork.CurrencyRates.GetRateByDateAsync(currencyId, date);
            if (rate == null)
                throw new BusinessException($"Missing exchange rate for currency {currency.Code} at or before {date.ToShortDateString()}.");

            if (rate.OfficialRate.HasValue && rate.OfficialRate.Value > 0)
                return rate.OfficialRate.Value;

            if (rate.SellRate > 0)
                return rate.SellRate;

            return rate.BuyRate;
        }

        public async Task<decimal> ConvertAsync(decimal amount, int fromCurrencyId, int toCurrencyId, DateTime date)
        {
            if (fromCurrencyId == toCurrencyId)
                return amount;

            var fromRate = await GetRateAsync(fromCurrencyId, date);
            var toRate = await GetRateAsync(toCurrencyId, date);

            // Convert to base currency then target currency
            var baseAmount = amount * fromRate;
            return baseAmount / toRate;
        }
    }
}
