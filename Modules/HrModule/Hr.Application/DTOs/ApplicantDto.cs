using Hr.Domain.Enums;
using System;
using System.Collections.Generic;

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
        public string? Skills { get; set; }
        // Removed EducationalQualifications field
        public DateTime? InterviewDate { get; set; }
        
        // New fields
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        
        public ICollection<ApplicantEducationDto> Educations { get; set; } = new List<ApplicantEducationDto>();
        public ICollection<ApplicantExperienceDto> Experiences { get; set; } = new List<ApplicantExperienceDto>();
        public ICollection<HrAttachmentDto> Attachments { get; set; } = new List<HrAttachmentDto>();
    }
}