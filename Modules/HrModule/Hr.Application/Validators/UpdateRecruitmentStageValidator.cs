using FluentValidation;
using Hr.Application.Features.RecruitmentStageFeatures.UpdateRecruitmentStage;

namespace Hr.Application.Validators
{
    public class UpdateRecruitmentStageValidator : AbstractValidator<UpdateRecruitmentStageRequest>
    {
        public UpdateRecruitmentStageValidator()
        {
            RuleFor(x => x.StageId)
                .GreaterThan(0).WithMessage("Stage ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Stage name is required")
                .MaximumLength(100).WithMessage("Stage name cannot exceed 100 characters");

            RuleFor(x => x.SequenceOrder)
                .GreaterThan(0).WithMessage("Sequence order must be greater than 0");
        }
    }
}