using Hr.Application.DTOs;

namespace Hr.Application.Features.ApplicantFeatures.CreateApplicant
{
    public class CreateApplicantResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ApplicantDto? Applicant { get; set; }
    }
}
