using FluentValidation;
using Hr.Application.Features.DepartmentFeatures.CreateDepartment;

namespace Hr.Application.Validators
{
    public class CreateDepartmentValidator : AbstractValidator<CreateDepartmentRequest>
    {
        public CreateDepartmentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
        }
    }
}
