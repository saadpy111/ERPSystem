using FluentValidation;
using Hr.Application.Features.RecruitmentStageFeatures.CreateRecruitmentStage;

namespace Hr.Application.Validators
{
    public class CreateRecruitmentStageValidator : AbstractValidator<CreateRecruitmentStageRequest>
    {
        public CreateRecruitmentStageValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Stage name is required")
                .MaximumLength(100).WithMessage("Stage name cannot exceed 100 characters");

            RuleFor(x => x.SequenceOrder)
                .GreaterThan(0).WithMessage("Sequence order must be greater than 0");
        }
    }
}
