using FluentValidation;
using Hr.Application.Features.LoanInstallmentFeatures.PayLoanInstallment;

namespace Hr.Application.Validators
{
    public class PayLoanInstallmentValidator : AbstractValidator<PayLoanInstallmentRequest>
    {
        public PayLoanInstallmentValidator()
        {
            RuleFor(x => x.InstallmentId)
                .GreaterThan(0).WithMessage("Installment ID is required");

            RuleFor(x => x.PaymentDate)
                .NotEmpty().WithMessage("Payment date is required");

            RuleFor(x => x.PaymentMethod)
                .MaximumLength(100).WithMessage("Payment method cannot exceed 100 characters");
        }
    }
}