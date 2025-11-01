using Hr.Application.DTOs;

namespace Hr.Application.Features.RecruitmentStageFeatures.CreateRecruitmentStage
{
    public class CreateRecruitmentStageResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public RecruitmentStageDto? RecruitmentStage { get; set; }
    }
}
