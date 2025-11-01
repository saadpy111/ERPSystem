using Hr.Application.DTOs;

namespace Hr.Application.Features.RecruitmentStageFeatures.UpdateRecruitmentStage
{
    public class UpdateRecruitmentStageResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? RecruitmentStage { get; set; }
    }
}
