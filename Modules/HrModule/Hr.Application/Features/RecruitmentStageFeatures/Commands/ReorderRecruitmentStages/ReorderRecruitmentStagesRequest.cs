using MediatR;

namespace Hr.Application.Features.RecruitmentStageFeatures.ReorderRecruitmentStages
{
    public class ReorderRecruitmentStagesRequest : IRequest<ReorderRecruitmentStagesResponse>
    {
        public List<StageOrder> StageOrders { get; set; } = new List<StageOrder>();
    }

    public class StageOrder
    {
        public int StageId { get; set; }
        public int SequenceOrder { get; set; }
    }
}
