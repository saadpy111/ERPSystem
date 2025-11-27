using FluentValidation;
using Hr.Application.Features.RecruitmentStageFeatures.ActivateRecruitmentStage;

namespace Hr.Application.Validators
{
    public class ActivateRecruitmentStageValidator : AbstractValidator<ActivateRecruitmentStageRequest>
    {
        public ActivateRecruitmentStageValidator()
        {
            RuleFor(x => x.StageId)
                .GreaterThan(0).WithMessage("Stage ID is required");
        }
    }
}