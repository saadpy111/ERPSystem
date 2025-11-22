using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class JobDto
    {
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public WorkType? WorkType { get; set; }
        public JobStatus Status { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsActive { get; set; }
        public int ApplicantsCount { get; set; }
        public string? Responsibilities { get; set; }
        public string? RequiredSkills { get; set; }
        public string? RequiredExperience { get; set; }
        public string? RequiredQualification { get; set; }
    }
}