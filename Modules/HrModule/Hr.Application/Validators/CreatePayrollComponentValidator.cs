using FluentValidation;
using Hr.Application.Features.PayrollComponentFeatures.CreatePayrollComponent;

namespace Hr.Application.Validators
{
    public class CreatePayrollComponentValidator : AbstractValidator<CreatePayrollComponentRequest>
    {
        public CreatePayrollComponentValidator()
        {
            RuleFor(x => x.PayrollRecordId)
                .GreaterThan(0).WithMessage("Payroll record is required");

            RuleFor(x => x.ComponentType)
                .IsInEnum().WithMessage("Valid component type is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Component name is required")
                .MaximumLength(100).WithMessage("Component name cannot exceed 100 characters");


        }
    }
}
