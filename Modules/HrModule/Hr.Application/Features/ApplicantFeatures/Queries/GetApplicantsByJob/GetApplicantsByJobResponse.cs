using Hr.Application.DTOs;

namespace Hr.Application.Features.ApplicantFeatures.GetApplicantsByJob
{
    public class GetApplicantsByJobResponse
    {
        public IEnumerable<ApplicantDto> Applicants { get; set; } = new List<ApplicantDto>();
    }
}
