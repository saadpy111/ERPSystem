using Accounting.Application.Features.Accounts.Commands.CreateAccount;
using FluentValidation;

namespace Accounting.Application.Features.Accounts.Validators
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CurrencyId).GreaterThan(0);
        }
    }
}
