using FluentValidation;
using Hr.Application.Features.LeaveRequestFeatures.DeleteLeaveRequest;

namespace Hr.Application.Validators
{
    public class DeleteLeaveRequestValidator : AbstractValidator<DeleteLeaveRequestRequest>
    {
        public DeleteLeaveRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Leave request ID is required");
        }
    }
}