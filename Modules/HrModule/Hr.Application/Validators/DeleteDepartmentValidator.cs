using FluentValidation;
using Hr.Application.Features.DepartmentFeatures.DeleteDepartment;

namespace Hr.Application.Validators
{
    public class DeleteDepartmentValidator : AbstractValidator<DeleteDepartmentRequest>
    {
        public DeleteDepartmentValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Department ID is required");
        }
    }
}