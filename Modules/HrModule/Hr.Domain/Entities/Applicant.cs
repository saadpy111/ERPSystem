using System;
using System.ComponentModel.DataAnnotations;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class Applicant : BaseEntity
    {
        [Key]
        public int ApplicantId { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public int JobId { get; set; }
        public Job AppliedJob { get; set; } = null!;

        public DateTime ApplicationDate { get; set; }

        [Required]
        public int CurrentStageId { get; set; }
        public RecruitmentStage CurrentStage { get; set; } = null!;

        public ApplicantStatus Status { get; set; } = ApplicantStatus.Applied;

        [StringLength(255)]
        public string? ResumeUrl { get; set; }

        public string? QualificationsDetails { get; set; }
        public string? ExperienceDetails { get; set; }
        
        [StringLength(1000)]
        public string? Skills { get; set; }
        
        [StringLength(1000)]
        public string? EducationalQualifications { get; set; }

        public DateTime? InterviewDate { get; set; }
        public string? InterviewNotes { get; set; }
    }
}