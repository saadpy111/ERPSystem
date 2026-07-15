using FluentValidation;

namespace Website.Application.Features.WalletFeatures.Commands.RequestWithdrawal
{
    public class RequestWithdrawalCommandValidator : AbstractValidator<RequestWithdrawalCommand>
    {
        public RequestWithdrawalCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Withdrawal amount must be greater than zero.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
