using FluentValidation;
using Hr.Application.Features.DepartmentFeatures.CreateDepartment;

namespace Hr.Application.Validators
{
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentRequest>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");
        }
    }
}
