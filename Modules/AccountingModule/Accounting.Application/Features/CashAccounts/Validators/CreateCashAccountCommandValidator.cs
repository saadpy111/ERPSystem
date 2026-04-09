using Accounting.Application.Features.CashAccounts.Commands.CreateCashAccount;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Domain.Enums;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Accounting.Application.Features.CashAccounts.Validators
{
    public class CreateCashAccountCommandValidator : AbstractValidator<CreateCashAccountCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCashAccountCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.AccountId).GreaterThan(0);

            // Move the inline checks here
            RuleFor(x => x.AccountId).MustAsync(AccountIsValid).WithMessage("Account is invalid or does not exist.");
        }

        private async Task<bool> AccountIsValid(int accountId, CancellationToken cancellationToken)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(accountId);
            if (account == null) return false;
            if (account.IsGroup) return false;
            if (account.AccountType != AccountType.Asset) return false;
            return true;
        }
    }
}
