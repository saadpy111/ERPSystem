using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.ActivateRecruitmentStage
{
    public class ActivateRecruitmentStageRequest : IRequest<ActivateRecruitmentStageResponse>
    {
        public int StageId { get; set; }
    }
}
