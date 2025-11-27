using FluentValidation;
using Hr.Application.Features.RecruitmentStageFeatures.DeactivateRecruitmentStage;

namespace Hr.Application.Validators
{
    public class DeactivateRecruitmentStageValidator : AbstractValidator<DeactivateRecruitmentStageRequest>
    {
        public DeactivateRecruitmentStageValidator()
        {
            RuleFor(x => x.StageId)
                .GreaterThan(0).WithMessage("Stage ID is required");
        }
    }
}