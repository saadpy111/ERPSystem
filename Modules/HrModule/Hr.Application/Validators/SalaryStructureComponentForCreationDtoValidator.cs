using FluentValidation;
using Hr.Application.DTOs;

namespace Hr.Application.Validators
{
    public class SalaryStructureComponentForCreationDtoValidator : AbstractValidator<SalaryStructureComponentForCreationDto>
    {
        public SalaryStructureComponentForCreationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Component name is required")
                .MaximumLength(100).WithMessage("Component name cannot exceed 100 characters");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Component type is required");

            RuleFor(x => x.FixedAmount)
                .GreaterThanOrEqualTo(0).When(x => x.FixedAmount.HasValue)
                .WithMessage("Fixed amount must be greater than or equal to 0");

            RuleFor(x => x.Percentage)
                .GreaterThanOrEqualTo(0).When(x => x.Percentage.HasValue)
                .WithMessage("Percentage must be greater than or equal to 0")
                .LessThanOrEqualTo(100).When(x => x.Percentage.HasValue)
                .WithMessage("Percentage must be less than or equal to 100");

            RuleFor(x => x)
                .Must(x => x.FixedAmount.HasValue || x.Percentage.HasValue)
                .WithMessage("Either Fixed Amount or Percentage must be provided");
        }
    }
}