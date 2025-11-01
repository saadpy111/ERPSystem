using Hr.Application.DTOs;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsByStage
{
    public class GetApplicantsByStageResponse
    {
        public IEnumerable<ApplicantDto> Applicants { get; set; } = new List<ApplicantDto>();
    }
}
