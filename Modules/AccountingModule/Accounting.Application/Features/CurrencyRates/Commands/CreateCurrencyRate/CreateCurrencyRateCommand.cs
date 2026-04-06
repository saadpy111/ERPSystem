using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Accounting.Application.Features.CurrencyRates.Commands.CreateCurrencyRate
{
    public class CreateCurrencyRateCommand : IRequest<int>
    {
        public int CurrencyId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
        public decimal? OfficialRate { get; set; }
        public string Source { get; set; } = null!;
    }

    public class CreateCurrencyRateCommandValidator : AbstractValidator<CreateCurrencyRateCommand>
    {
        public CreateCurrencyRateCommandValidator()
        {
            RuleFor(x => x.CurrencyId).GreaterThan(0);
            RuleFor(x => x.BuyRate).GreaterThan(0);
            RuleFor(x => x.SellRate).GreaterThan(0);
            RuleFor(x => x.EffectiveDate).NotEmpty();
            RuleFor(x => x.Source).NotEmpty().MaximumLength(200);
        }
    }

    public class CreateCurrencyRateCommandHandler : IRequestHandler<CreateCurrencyRateCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCurrencyRateCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateCurrencyRateCommand request, CancellationToken cancellationToken)
        {
            var currency = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId);
            if (currency == null)
                throw new BusinessException("Currency not found.");

            if (currency.IsBaseCurrency)
                throw new BusinessException("Cannot set exchange rate for native Base Currency.");

            if (await _unitOfWork.CurrencyRates.ExistsForDateAsync(request.CurrencyId, request.EffectiveDate))
                throw new BusinessException($"An exchange rate already exists for {currency.Code} on {request.EffectiveDate.ToShortDateString()}. Updates are prohibited for historical integrity.");

            var rate = new CurrencyRate
            {
                CurrencyId = request.CurrencyId,
                EffectiveDate = request.EffectiveDate.Date,
                BuyRate = request.BuyRate,
                SellRate = request.SellRate,
                OfficialRate = request.OfficialRate,
                Source = request.Source,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.CurrencyRates.AddAsync(rate);
            await _unitOfWork.SaveChangesAsync();

            return rate.Id;
        }
    }
}
