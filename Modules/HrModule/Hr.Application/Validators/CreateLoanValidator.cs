using FluentValidation;
using Hr.Application.Features.LoanFeatures.CreateLoan;

namespace Hr.Application.Validators
{
    public class CreateLoanValidator : AbstractValidator<CreateLoanRequest>
    {
        public CreateLoanValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee is required");

            RuleFor(x => x.PrincipalAmount)
                .GreaterThan(0).WithMessage("Loan amount must be greater than 0");

            RuleFor(x => x.MonthlyInstallment)
                .GreaterThan(0).WithMessage("Monthly installment must be greater than 0");

            RuleFor(x => x.TermMonths)
                .GreaterThan(0).WithMessage("Term months must be greater than 0");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");
        }
    }
}
