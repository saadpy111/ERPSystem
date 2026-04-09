using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Currencies.Commands.CreateCurrency
{
    public class CreateCurrencyCommand : IRequest<Result<int>>
    {
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public int DecimalPlaces { get; set; }
    }

    public class CreateCurrencyCommandValidator : AbstractValidator<CreateCurrencyCommand>
    {
        public CreateCurrencyCommandValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(10);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Symbol).NotEmpty().MaximumLength(10);
            RuleFor(x => x.DecimalPlaces).GreaterThanOrEqualTo(0);
        }
    }

    public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCurrencyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Currencies.IsCodeUniqueAsync(request.Code))
            {
                throw new BusinessException($"Currency Code '{request.Code}' already exists.");
            }

            var baseCurrencyExists = await _unitOfWork.Currencies.GetBaseCurrencyAsync() != null;

            var currency = new Currency
            {
                Code = request.Code,
                NameAr = request.NameAr,
                NameEn = request.NameEn,
                Symbol = request.Symbol,
                DecimalPlaces = request.DecimalPlaces,
                IsActive = true,
                IsBaseCurrency = !baseCurrencyExists // Make it base if it is the first one
            };

            await _unitOfWork.Currencies.AddAsync(currency);
            await _unitOfWork.SaveChangesAsync();

            return currency.Id;
        }
    }
}
