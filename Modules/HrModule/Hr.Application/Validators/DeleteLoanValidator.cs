using FluentValidation;
using Hr.Application.Features.LoanFeatures.DeleteLoan;

namespace Hr.Application.Validators
{
    public class DeleteLoanValidator : AbstractValidator<DeleteLoanRequest>
    {
        public DeleteLoanValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Loan ID is required");
        }
    }
}