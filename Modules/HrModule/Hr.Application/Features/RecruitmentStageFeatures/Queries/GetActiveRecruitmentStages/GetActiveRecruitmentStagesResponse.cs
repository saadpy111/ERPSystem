using Hr.Application.DTOs;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetActiveRecruitmentStages
{
    public class GetActiveRecruitmentStagesResponse
    {
        public IEnumerable<RecruitmentStageDto> Stages { get; set; } = new List<RecruitmentStageDto>();
    }
}
