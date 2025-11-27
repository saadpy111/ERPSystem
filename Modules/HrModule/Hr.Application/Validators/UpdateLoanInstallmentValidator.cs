using FluentValidation;
using Hr.Application.Features.LoanInstallmentFeatures.UpdateLoanInstallment;

namespace Hr.Application.Validators
{
    public class UpdateLoanInstallmentValidator : AbstractValidator<UpdateLoanInstallmentRequest>
    {
        public UpdateLoanInstallmentValidator()
        {
            RuleFor(x => x.InstallmentId)
                .GreaterThan(0).WithMessage("Installment ID is required");

            RuleFor(x => x.LoanId)
                .GreaterThan(0).WithMessage("Loan ID is required");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required");

            RuleFor(x => x.AmountDue)
                .GreaterThan(0).WithMessage("Amount due must be greater than 0");
        }
    }
}