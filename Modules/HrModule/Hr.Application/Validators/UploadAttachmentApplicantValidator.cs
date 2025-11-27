using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.Commands.UploadAttachmentApplicant;

namespace Hr.Application.Validators
{
    public class UploadAttachmentApplicantValidator : AbstractValidator<UploadAttachmentApplicantRequest>
    {
        public UploadAttachmentApplicantValidator()
        {
            RuleFor(x => x.ApplicantId)
                .GreaterThan(0).WithMessage("Applicant ID is required");

            RuleFor(x => x.AttachmentFiles)
                .NotNull().WithMessage("Attachment files are required")
                .Must(files => files.Any()).WithMessage("At least one attachment file is required");
        }
    }
}