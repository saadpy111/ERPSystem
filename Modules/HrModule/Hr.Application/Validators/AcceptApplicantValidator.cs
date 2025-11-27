using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.AcceptApplicant;

namespace Hr.Application.Validators
{
    public class AcceptApplicantValidator : AbstractValidator<AcceptApplicantRequest>
    {
        public AcceptApplicantValidator()
        {
            RuleFor(x => x.ApplicantId)
                .GreaterThan(0).WithMessage("Applicant ID is required");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");
        }
    }
}