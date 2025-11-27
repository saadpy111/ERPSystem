using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.DeleteApplicant;

namespace Hr.Application.Validators
{
    public class DeleteApplicantValidator : AbstractValidator<DeleteApplicantRequest>
    {
        public DeleteApplicantValidator()
        {
            RuleFor(x => x.ApplicantId)
                .GreaterThan(0).WithMessage("Applicant ID is required");
        }
    }
}