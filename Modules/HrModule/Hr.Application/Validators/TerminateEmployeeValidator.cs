using FluentValidation;
using Hr.Application.Features.EmployeeFeatures.TerminateEmployee;

namespace Hr.Application.Validators
{
    public class TerminateEmployeeValidator : AbstractValidator<TerminateEmployeeRequest>
    {
        public TerminateEmployeeValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID is required");
        }
    }
}