using FluentValidation;
using Hr.Application.Features.PayrollComponentFeatures.UpdatePayrollComponent;

namespace Hr.Application.Validators
{
    public class UpdatePayrollComponentValidator : AbstractValidator<UpdatePayrollComponentRequest>
    {
        public UpdatePayrollComponentValidator()
        {
            RuleFor(x => x.ComponentId)
                .GreaterThan(0).WithMessage("Component ID is required");

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