using Hr.Application.DTOs;

namespace Hr.Application.Features.ApplicantFeatures.GetAllApplicants
{
    public class GetAllApplicantsResponse
    {
        public IEnumerable<ApplicantDto> Applicants { get; set; } = new List<ApplicantDto>();
    }
}
