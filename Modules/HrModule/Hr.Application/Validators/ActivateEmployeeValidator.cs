using FluentValidation;
using Hr.Application.Features.EmployeeFeatures.ActivateEmployee;

namespace Hr.Application.Validators
{
    public class ActivateEmployeeValidator : AbstractValidator<ActivateEmployeeRequest>
    {
        public ActivateEmployeeValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID is required");
        }
    }
}