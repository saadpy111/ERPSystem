using FluentValidation;

namespace Accounting.Application.Features.Budgets.Commands.CreateBudget
{
    public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
    {
        public CreateBudgetCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

            RuleFor(v => v.FiscalYearId)
                .NotEmpty().WithMessage("Fiscal Year is required.");

            RuleForEach(v => v.Lines).SetValidator(new BudgetLineRequestValidator());
        }
    }

    public class BudgetLineRequestValidator : AbstractValidator<BudgetLineRequest>
    {
        public BudgetLineRequestValidator()
        {
            RuleFor(v => v.AccountId)
                .NotEmpty().WithMessage("Account is required.");

            RuleFor(v => v.PlannedAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Planned amount must be greater than or equal to zero.");

            RuleFor(v => v.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(v => v.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(v => v.StartDate).WithMessage("End date must be after start date.");
        }
    }
}
