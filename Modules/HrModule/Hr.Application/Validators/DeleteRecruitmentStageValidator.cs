using FluentValidation;
using Hr.Application.Features.RecruitmentStageFeatures.DeleteRecruitmentStage;

namespace Hr.Application.Validators
{
    public class DeleteRecruitmentStageValidator : AbstractValidator<DeleteRecruitmentStageRequest>
    {
        public DeleteRecruitmentStageValidator()
        {
            RuleFor(x => x.StageId)
                .GreaterThan(0).WithMessage("Stage ID is required");
        }
    }
}