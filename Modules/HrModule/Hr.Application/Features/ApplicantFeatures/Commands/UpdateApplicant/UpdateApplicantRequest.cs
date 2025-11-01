using Hr.Domain.Enums;
using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.UpdateApplicant
{
    public class UpdateApplicantRequest : IRequest<UpdateApplicantResponse>
    {
        public int ApplicantId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int JobId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int CurrentStageId { get; set; }
        public ApplicantStatus Status { get; set; }
        public string? ResumeUrl { get; set; }
        public string? QualificationsDetails { get; set; }
        public string? ExperienceDetails { get; set; }
    }
}
