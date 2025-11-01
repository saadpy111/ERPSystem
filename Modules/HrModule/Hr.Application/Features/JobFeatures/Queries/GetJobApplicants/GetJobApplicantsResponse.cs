using Hr.Application.DTOs;

namespace Hr.Application.Features.JobFeatures.GetJobApplicants
{
    public class GetJobApplicantsResponse
    {
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public IEnumerable<ApplicantDto> Applicants { get; set; } = new List<ApplicantDto>();
    }
}
