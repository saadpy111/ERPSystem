using Hr.Application.DTOs;
using Hr.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

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
        // Removed ExperienceDetails field
        public string? Skills { get; set; }
        // Removed EducationalQualifications field
        
        // New fields
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        
        public ICollection<ApplicantEducationDto> Educations { get; set; } = new List<ApplicantEducationDto>();
        public ICollection<ApplicantExperienceDto> Experiences { get; set; } = new List<ApplicantExperienceDto>();
        public ICollection<IFormFile>? AttachmentFiles { get; set; }
    }
}