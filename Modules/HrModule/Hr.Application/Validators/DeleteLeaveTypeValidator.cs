using FluentValidation;
using Hr.Application.Features.LeaveTypeFeatures.Commands.DeleteLeaveType;

namespace Hr.Application.Validators
{
    public class DeleteLeaveTypeValidator : AbstractValidator<DeleteLeaveTypeRequest>
    {
        public DeleteLeaveTypeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Leave type ID is required");
        }
    }
}