using FluentValidation;
using Hr.Application.Features.ApplicantFeatures.MoveApplicantToStage;

namespace Hr.Application.Validators
{
    public class MoveApplicantToStageValidator : AbstractValidator<MoveApplicantToStageRequest>
    {
        public MoveApplicantToStageValidator()
        {
            RuleFor(x => x.ApplicantId)
                .GreaterThan(0).WithMessage("Applicant ID is required");

            RuleFor(x => x.NewStageId)
                .GreaterThan(0).WithMessage("New stage ID is required");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");
        }
    }
}