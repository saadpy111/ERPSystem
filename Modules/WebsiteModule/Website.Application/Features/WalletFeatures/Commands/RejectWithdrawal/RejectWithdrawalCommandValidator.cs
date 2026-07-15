using FluentValidation;

namespace Website.Application.Features.WalletFeatures.Commands.RejectWithdrawal
{
    public class RejectWithdrawalCommandValidator : AbstractValidator<RejectWithdrawalCommand>
    {
        public RejectWithdrawalCommandValidator()
        {
            RuleFor(x => x.WithdrawalRequestId)
                .NotEmpty().WithMessage("Withdrawal request ID is required.");

            RuleFor(x => x.ReviewedBy)
                .NotEmpty().WithMessage("Reviewer ID is required.");
        }
    }
}
