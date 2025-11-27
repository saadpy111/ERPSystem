using FluentValidation;
using Hr.Application.Features.EmployeeFeatures.DeleteEmployee;

namespace Hr.Application.Validators
{
    public class DeleteEmployeeValidator : AbstractValidator<DeleteEmployeeRequest>
    {
        public DeleteEmployeeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Employee ID is required");
        }
    }
}