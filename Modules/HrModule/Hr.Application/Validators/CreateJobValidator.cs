using FluentValidation;
using Hr.Application.Features.JobFeatures.CreateJob;

namespace Hr.Application.Validators
{
    public class CreateJobValidator : AbstractValidator<CreateJobRequest>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job title is required")
                .MaximumLength(100).WithMessage("Job title cannot exceed 100 characters");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Department is required");

            RuleFor(x => x.PublishedDate)
                .NotEmpty().WithMessage("Published date is required");

            RuleFor(x => x.Responsibilities)
                .MaximumLength(2000).WithMessage("Responsibilities cannot exceed 2000 characters");

            RuleFor(x => x.RequiredSkills)
                .MaximumLength(2000).WithMessage("Required skills cannot exceed 2000 characters");

            RuleFor(x => x.RequiredExperience)
                .MaximumLength(2000).WithMessage("Required experience cannot exceed 2000 characters");

            RuleFor(x => x.RequiredQualification)
                .MaximumLength(2000).WithMessage("Required qualification cannot exceed 2000 characters");
        }
    }
}