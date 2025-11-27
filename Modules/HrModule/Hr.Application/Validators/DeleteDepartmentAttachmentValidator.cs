using FluentValidation;
using Hr.Application.Features.DepartmentFeatures.Commands.DeleteAttachment;

namespace Hr.Application.Validators
{
    public class DeleteDepartmentAttachmentValidator : AbstractValidator<DeleteDepartmentAttachmentRequest>
    {
        public DeleteDepartmentAttachmentValidator()
        {
            RuleFor(x => x.AttachmentId)
                .GreaterThan(0).WithMessage("Attachment ID is required");
        }
    }
}