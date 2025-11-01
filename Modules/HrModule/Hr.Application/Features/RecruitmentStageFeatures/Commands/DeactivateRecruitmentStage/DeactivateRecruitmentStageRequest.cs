using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.DeactivateRecruitmentStage
{
    public class DeactivateRecruitmentStageRequest : IRequest<DeactivateRecruitmentStageResponse>
    {
        public int StageId { get; set; }
    }
}
