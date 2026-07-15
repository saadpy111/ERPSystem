using FluentValidation;

namespace Website.Application.Features.WalletFeatures.Commands.ApproveWithdrawal
{
    public class ApproveWithdrawalCommandValidator : AbstractValidator<ApproveWithdrawalCommand>
    {
        public ApproveWithdrawalCommandValidator()
        {
            RuleFor(x => x.WithdrawalRequestId)
                .NotEmpty().WithMessage("Withdrawal request ID is required.");

            RuleFor(x => x.ReviewedBy)
                .NotEmpty().WithMessage("Reviewer ID is required.");
        }
    }
}
