using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.ApplicantFeatures.CreateApplicant
{
    public class CreateApplicantRequest : IRequest<CreateApplicantResponse>
    {
        public string FullName { get; set; } = string.Empty;
        public int JobId { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int CurrentStageId { get; set; }
        public string? ResumeUrl { get; set; }
        public string? QualificationsDetails { get; set; }
        public string? ExperienceDetails { get; set; }
        public string? Skills { get; set; }
        public string? EducationalQualifications { get; set; }
        public ICollection<IFormFile>? AttachmentFiles { get; set; }
    }
}