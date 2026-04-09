using Accounting.Application.Features.Receivables.Commands;
using Accounting.Application.Interfaces.Repositories;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.Receivables.Validators
{
    public class ReceivePaymentCommandValidator : AbstractValidator<ReceivePaymentCommand>
    {
        private readonly IUnitOfWork _uow;

        public ReceivePaymentCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero");

            RuleFor(x => x.ReceivableId)
                .MustAsync(ReceivableExists)
                .WithMessage("Receivable not found");

            RuleFor(x => x.CashAccountId)
                .MustAsync(CashAccountExists)
                .WithMessage("CashAccount not found");

            RuleFor(x => x)
                .MustAsync(NotExceedRemainingAmount)
                .WithMessage("Payment cannot exceed remaining amount");
        }

        private async Task<bool> ReceivableExists(int receivableId, CancellationToken token)
        {
            var receivable = await _uow.Receivables.GetByIdAsync(receivableId);
            return receivable != null;
        }

        private async Task<bool> CashAccountExists(int cashAccountId, CancellationToken token)
        {
            var cashAccount = await _uow.CashAccounts.GetByIdAsync(cashAccountId);
            return cashAccount != null;
        }

        private async Task<bool> NotExceedRemainingAmount(ReceivePaymentCommand req, CancellationToken token)
        {
            var receivable = await _uow.Receivables.GetByIdAsync(req.ReceivableId);
            if (receivable == null) return true; // Let ReceivableExists fail instead
            return req.Amount <= receivable.RemainingAmount;
        }
    }
}
