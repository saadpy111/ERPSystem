using FluentValidation;
using Hr.Application.Features.LeaveTypeFeatures.Commands.CreateLeaveType;

namespace Hr.Application.Validators
{
    public class CreateLeaveTypeValidator : AbstractValidator<CreateLeaveTypeRequest>
    {
        public CreateLeaveTypeValidator()
        {
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