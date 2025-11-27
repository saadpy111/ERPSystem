using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.RejectApplicant;

namespace Hr.Application.Validators
{
    public class RejectApplicantValidator : AbstractValidator<RejectApplicantRequest>
    {
        public RejectApplicantValidator()
        {
            RuleFor(x => x.ApplicantId)
                .GreaterThan(0).WithMessage("Applicant ID is required");

            RuleFor(x => x.RejectionReason)
                .MaximumLength(1000).WithMessage("Rejection reason cannot exceed 1000 characters");
        }
    }
}