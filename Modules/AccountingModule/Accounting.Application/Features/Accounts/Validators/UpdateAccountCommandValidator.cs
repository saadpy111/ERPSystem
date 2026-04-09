using Accounting.Application.Features.Accounts.Commands.UpdateAccount;
using FluentValidation;

namespace Accounting.Application.Features.Accounts.Validators
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.NameAr).NotEmpty().MaximumLength(100);
        }
    }
}
