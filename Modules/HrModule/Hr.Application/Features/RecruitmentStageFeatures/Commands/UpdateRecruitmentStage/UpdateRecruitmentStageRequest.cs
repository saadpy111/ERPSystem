using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.UpdateRecruitmentStage
{
    public class UpdateRecruitmentStageRequest : IRequest<UpdateRecruitmentStageResponse>
    {
        public int StageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SequenceOrder { get; set; }
    }
}
