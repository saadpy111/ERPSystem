using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.CreateApplicant;

namespace Hr.Application.Validators
{
    public class CreateApplicantValidator : AbstractValidator<CreateApplicantRequest>
    {
        public CreateApplicantValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.JobId)
                .GreaterThan(0).WithMessage("Job is required");

            RuleFor(x => x.ApplicationDate)
                .NotEmpty().WithMessage("Application date is required");

            RuleFor(x => x.ResumeUrl)
                .MaximumLength(500).WithMessage("Resume URL cannot exceed 500 characters");

            RuleFor(x => x.QualificationsDetails)
                .MaximumLength(1000).WithMessage("Qualifications details cannot exceed 1000 characters");

            RuleFor(x => x.ExperienceDetails)
                .MaximumLength(1000).WithMessage("Experience details cannot exceed 1000 characters");
        }
    }
}
