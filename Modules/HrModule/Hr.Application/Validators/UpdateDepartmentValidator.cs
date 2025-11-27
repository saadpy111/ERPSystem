using FluentValidation;
using Hr.Application.Features.DepartmentFeatures.UpdateDepartment;

namespace Hr.Application.Validators
{
    public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentRequest>
    {
        public UpdateDepartmentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Department ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
        }
    }
}