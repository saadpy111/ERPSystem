using System;
using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Accounting.Application.Features.Currencies.Commands.UpdateCurrency
{
    public class UpdateCurrencyCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameAr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public int DecimalPlaces { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
    {
        public UpdateCurrencyCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(10);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(200);
            RuleFor(x => x.NameEn).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Symbol).NotEmpty().MaximumLength(10);
            RuleFor(x => x.DecimalPlaces).GreaterThanOrEqualTo(0);
        }
    }

    public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCurrencyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = await _unitOfWork.Currencies.GetByIdAsync(request.Id);
            if (currency == null)
                throw new BusinessException("Currency not found.");

            if (currency.Code != request.Code)
            {
                if (!await _unitOfWork.Currencies.IsCodeUniqueAsync(request.Code))
                {
                    throw new BusinessException($"Currency Code '{request.Code}' already exists.");
                }
            }

            if (!request.IsActive && currency.IsBaseCurrency)
            {
                throw new BusinessException("Cannot deactivate the Base Currency. Set another currency as base first.");
            }

            currency.Code = request.Code;
            currency.NameAr = request.NameAr;
            currency.NameEn = request.NameEn;
            currency.Symbol = request.Symbol;
            currency.DecimalPlaces = request.DecimalPlaces;
            currency.IsActive = request.IsActive;
            currency.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Currencies.Update(currency);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
