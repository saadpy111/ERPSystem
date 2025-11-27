using FluentValidation;
using Hr.Application.Features.LeaveRequestFeatures.UpdateLeaveRequest;

namespace Hr.Application.Validators
{
    public class UpdateLeaveRequestValidator : AbstractValidator<UpdateLeaveRequestRequest>
    {
        public UpdateLeaveRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Leave request ID is required");

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee is required");

            RuleFor(x => x.LeaveTypeId)
                .GreaterThan(0).WithMessage("Leave type is required");

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