using FluentValidation;
using Hr.Application.Features.EmployeeContractFeatures.Commands.DeleteAttachmentContract;

namespace Hr.Application.Validators
{
    public class DeleteAttachmentContractValidator : AbstractValidator<DeleteAttachmentContractRequest>
    {
        public DeleteAttachmentContractValidator()
        {
            RuleFor(x => x.AttachmentId)
                .GreaterThan(0).WithMessage("Attachment ID is required");
        }
    }
}