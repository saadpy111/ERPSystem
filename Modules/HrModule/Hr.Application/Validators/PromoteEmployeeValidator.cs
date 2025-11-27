using FluentValidation;
using Hr.Application.Features.EmployeeFeatures.PromoteEmployee;

namespace Hr.Application.Validators
{
    public class PromoteEmployeeValidator : AbstractValidator<PromoteEmployeeRequest>
    {
        public PromoteEmployeeValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID is required");
        }
    }
}