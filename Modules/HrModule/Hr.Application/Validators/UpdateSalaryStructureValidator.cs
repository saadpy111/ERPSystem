using FluentValidation;
using Hr.Application.Features.SalaryStructureFeatures.Commands.UpdateSalaryStructure;

namespace Hr.Application.Validators
{
    public class UpdateSalaryStructureValidator : AbstractValidator<UpdateSalaryStructureRequest>
    {
        public UpdateSalaryStructureValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(150).WithMessage("Name cannot exceed 150 characters");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(50).WithMessage("Code cannot exceed 50 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required");

            // Validate each component in the collection
            RuleForEach(x => x.Components)
                .SetValidator(new SalaryStructureComponentForUpdateDtoValidator());
        }
    }
}