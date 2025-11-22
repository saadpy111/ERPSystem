using FluentValidation;
using Hr.Application.Features.SalaryStructureFeatures.Commands.CreateSalaryStructure;

namespace Hr.Application.Validators
{
    public class CreateSalaryStructureValidator : AbstractValidator<CreateSalaryStructureRequest>
    {
        public CreateSalaryStructureValidator()
        {
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
                .SetValidator(new SalaryStructureComponentForCreationDtoValidator());
        }
    }
}