using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.CreateRecruitmentStage
{
    public class CreateRecruitmentStageRequest : IRequest<CreateRecruitmentStageResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SequenceOrder { get; set; }
    }
}
