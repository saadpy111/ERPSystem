using FluentValidation;
using Hr.Application.Features.LeaveRequestFeatures.CreateLeaveRequest;

namespace Hr.Application.Validators
{
    public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestRequest>
    {
        public CreateLeaveRequestValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee is required");

            RuleFor(x => x.LeaveType)
                .IsInEnum().WithMessage("Valid leave type is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be greater than or equal to start date");

            RuleFor(x => x.DurationDays)
                .GreaterThan(0).WithMessage("Duration days must be greater than 0");
        }
    }
}
