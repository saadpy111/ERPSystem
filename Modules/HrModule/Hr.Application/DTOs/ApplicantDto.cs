using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class ApplicantDto
    {
        public int ApplicantId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public int CurrentStageId { get; set; }
        public string CurrentStageName { get; set; } = string.Empty;
        public ApplicantStatus Status { get; set; }
        public string? ResumeUrl { get; set; }
        public DateTime? InterviewDate { get; set; }
        public ICollection<HrAttachmentDto> Attachments { get; set; } = new List<HrAttachmentDto>();
    }
}
