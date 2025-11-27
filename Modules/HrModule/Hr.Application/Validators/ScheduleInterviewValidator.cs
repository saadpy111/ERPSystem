using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.ScheduleInterview;

namespace Hr.Application.Validators
{
    public class ScheduleInterviewValidator : AbstractValidator<ScheduleInterviewRequest>
    {
        public ScheduleInterviewValidator()
        {
            RuleFor(x => x.ApplicantId)
                .GreaterThan(0).WithMessage("Applicant ID is required");

            RuleFor(x => x.InterviewDate)
                .NotEmpty().WithMessage("Interview date is required");

            RuleFor(x => x.InterviewNotes)
                .MaximumLength(1000).WithMessage("Interview notes cannot exceed 1000 characters");
        }
    }
}