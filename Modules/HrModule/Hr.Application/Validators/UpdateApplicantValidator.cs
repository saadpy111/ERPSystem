using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.UpdateApplicant;

namespace Hr.Application.Validators
{
    public class UpdateApplicantValidator : AbstractValidator<UpdateApplicantRequest>
    {
        public UpdateApplicantValidator()
        {
            RuleFor(x => x.ApplicantId)
                .GreaterThan(0).WithMessage("Applicant ID is required");

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

            // Removed validation for ExperienceDetails
            // Removed validation for EducationalQualifications

            // Validate each experience in the collection
            RuleForEach(x => x.Experiences)
                .SetValidator(new ApplicantExperienceValidator());
        }
    }
}