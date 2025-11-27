using FluentValidation;
using Hr.Application.Features.LoanInstallmentFeatures.DeleteLoanInstallment;

namespace Hr.Application.Validators
{
    public class DeleteLoanInstallmentValidator : AbstractValidator<DeleteLoanInstallmentRequest>
    {
        public DeleteLoanInstallmentValidator()
        {
            RuleFor(x => x.InstallmentId)
                .GreaterThan(0).WithMessage("Installment ID is required");
        }
    }
}