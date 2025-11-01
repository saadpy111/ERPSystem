using Hr.Application.DTOs;

namespace Hr.Application.Features.RecruitmentStageFeatures.GetAllRecruitmentStages
{
    public class GetAllRecruitmentStagesResponse
    {
        public IEnumerable<RecruitmentStageDto> RecruitmentStages { get; set; } = new List<RecruitmentStageDto>();
    }
}
