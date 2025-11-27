using FluentValidation;
using Hr.Application.Features.EmployeeContractFeatures.Commands.UploadAttachmentContract;

namespace Hr.Application.Validators
{
    public class UploadAttachmentContractValidator : AbstractValidator<UploadAttachmentContractRequest>
    {
        public UploadAttachmentContractValidator()
        {
            RuleFor(x => x.EmployeeContractId)
                .GreaterThan(0).WithMessage("Employee contract ID is required");

            RuleFor(x => x.AttachmentFiles)
                .NotNull().WithMessage("Attachment files are required")
                .Must(files => files.Any()).WithMessage("At least one attachment file is required");
        }
    }
}