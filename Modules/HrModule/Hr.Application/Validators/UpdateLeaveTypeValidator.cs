using FluentValidation;
using Hr.Application.Features.LeaveTypeFeatures.Commands.UpdateLeaveType;

namespace Hr.Application.Validators
{
    public class UpdateLeaveTypeValidator : AbstractValidator<UpdateLeaveTypeRequest>
    {
        public UpdateLeaveTypeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Leave type ID is required");

            RuleFor(x => x.LeaveTypeName)
                .NotEmpty().WithMessage("Leave type name is required")
                .MaximumLength(100).WithMessage("Leave type name cannot exceed 100 characters");

            RuleFor(x => x.DurationDays)
                .GreaterThanOrEqualTo(0).WithMessage("Duration days must be greater than or equal to 0");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");
        }
    }
}