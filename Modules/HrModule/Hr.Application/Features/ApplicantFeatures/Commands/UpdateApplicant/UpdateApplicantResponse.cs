using Hr.Application.DTOs;

namespace Hr.Application.Features.ApplicantFeatures.UpdateApplicant
{
    public class UpdateApplicantResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ApplicantDto? Applicant { get; set; }
    }
}
