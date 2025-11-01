using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.DeleteRecruitmentStage
{
    public class DeleteRecruitmentStageRequest : IRequest<DeleteRecruitmentStageResponse>
    {
        public int StageId { get; set; }
    }
}
