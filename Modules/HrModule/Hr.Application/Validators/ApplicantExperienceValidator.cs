using FluentValidation;
using Hr.Application.DTOs;

namespace Hr.Application.Validators
{
    public class ApplicantExperienceValidator : AbstractValidator<ApplicantExperienceDto>
    {
        public ApplicantExperienceValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required")
                .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters");

            RuleFor(x => x.JobTitle)
                .NotEmpty().WithMessage("Job title is required")
                .MaximumLength(200).WithMessage("Job title cannot exceed 200 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .Must((dto, endDate) => endDate == null || endDate >= dto.StartDate)
                .WithMessage("End date must be after or equal to start date");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
        }
    }
}