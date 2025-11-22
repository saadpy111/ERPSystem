using System;
using System.Collections.Generic;
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
        // Removed ExperienceDetails field
        
        [StringLength(1000)]
        public string? Skills { get; set; }
        
        // Removed EducationalQualifications field

        public DateTime? InterviewDate { get; set; }
        public string? InterviewNotes { get; set; }
        
        // New fields
        [StringLength(500)]
        public string? Address { get; set; }
        
        public Gender Gender { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(255)]
        public string? Email { get; set; }
        
        // Navigation property for educations
        public ICollection<ApplicantEducation> Educations { get; set; } = new List<ApplicantEducation>();
        
        // Navigation property for experiences
        public ICollection<ApplicantExperience> Experiences { get; set; } = new List<ApplicantExperience>();
    }
}