using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.Commands.DeleteAttachmentApplicant;

namespace Hr.Application.Validators
{
    public class DeleteAttachmentApplicantValidator : AbstractValidator<DeleteAttachmentApplicantRequest>
    {
        public DeleteAttachmentApplicantValidator()
        {
            RuleFor(x => x.AttachmentId)
                .GreaterThan(0).WithMessage("Attachment ID is required");
        }
    }
}