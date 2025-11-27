using FluentValidation;
using Hr.Application.Features.DepartmentFeatures.Commands.UploadAttachment;

namespace Hr.Application.Validators
{
    public class UploadDepartmentAttachmentValidator : AbstractValidator<UploadDepartmentAttachmentRequest>
    {
        public UploadDepartmentAttachmentValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department ID is required");

            RuleFor(x => x.AttachmentFile)
                .NotNull().WithMessage("Attachment file is required");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
        }
    }
}