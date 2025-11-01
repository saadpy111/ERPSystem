using FluentValidation;
using Hr.Application.Features.LoanInstallmentFeatures.CreateLoanInstallment;

namespace Hr.Application.Validators
{
    public class CreateLoanInstallmentValidator : AbstractValidator<CreateLoanInstallmentRequest>
    {
        public CreateLoanInstallmentValidator()
        {
            RuleFor(x => x.LoanId)
                .GreaterThan(0).WithMessage("Loan is required");

            RuleFor(x => x.AmountDue)
                .GreaterThan(0).WithMessage("Amount must be greater than 0");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required");

            RuleFor(x => x.PaymentMethod)
                .MaximumLength(50).WithMessage("Payment method cannot exceed 50 characters");
        }
    }
}
