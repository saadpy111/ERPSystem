using System.Threading;
using System.Threading.Tasks;
using Accounting.Application.Common.Exceptions;
using Accounting.Application.Interfaces.Repositories;
using MediatR;

using Accounting.Application.Common.Models;

namespace Accounting.Application.Features.Currencies.Commands.SetBaseCurrency
{
    public class SetBaseCurrencyCommand : IRequest<Result<bool>>
    {
        public int CurrencyId { get; set; }
    }

    public class SetBaseCurrencyCommandHandler : IRequestHandler<SetBaseCurrencyCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetBaseCurrencyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(SetBaseCurrencyCommand request, CancellationToken cancellationToken)
        {
            var newBase = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId);
            if (newBase == null)
                return Result<bool>.Failure("Currency not found.");

            if (!newBase.IsActive)
                return Result<bool>.Failure("Cannot set inactive currency as base currency.");

            if (newBase.IsBaseCurrency)
                return true;

            var currentBase = await _unitOfWork.Currencies.GetBaseCurrencyAsync();
            if (currentBase != null)
            {
                currentBase.IsBaseCurrency = false;
                _unitOfWork.Currencies.Update(currentBase);
            }

            newBase.IsBaseCurrency = true;
            _unitOfWork.Currencies.Update(newBase);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
