using Hr.Application.DTOs;

namespace Hr.Application.Features.RecruitmentStageFeatures.ReorderRecruitmentStages
{
    public class ReorderRecruitmentStagesResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<RecruitmentStageDto> ReorderedStages { get; set; } = new List<RecruitmentStageDto>();
    }
}
