using Hr.Application.DTOs;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetOrderedRecruitmentStages
{
    public class GetOrderedRecruitmentStagesResponse
    {
        public IEnumerable<RecruitmentStageDto> Stages { get; set; } = new List<RecruitmentStageDto>();
    }
}
