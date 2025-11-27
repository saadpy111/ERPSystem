using FluentValidation;
using Hr.Application.Features.RecruitmentStageFeatures.ReorderRecruitmentStages;

namespace Hr.Application.Validators
{
    public class ReorderRecruitmentStagesValidator : AbstractValidator<ReorderRecruitmentStagesRequest>
    {
        public ReorderRecruitmentStagesValidator()
        {
            RuleFor(x => x.StageOrders)
                .NotNull().WithMessage("Stage orders are required")
                .Must(orders => orders.Any()).WithMessage("At least one stage order is required");

            RuleForEach(x => x.StageOrders)
                .ChildRules(stageOrder => {
                    stageOrder.RuleFor(s => s.StageId)
                        .GreaterThan(0).WithMessage("Stage ID is required");

                    stageOrder.RuleFor(s => s.SequenceOrder)
                        .GreaterThan(0).WithMessage("Sequence order must be greater than 0");
                });
        }
    }
}