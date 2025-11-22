using System;
using System.ComponentModel.DataAnnotations;
using Hr.Domain;

namespace Hr.Domain.Entities
{
    public class ApplicantExperience : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        // Foreign key
        public int ApplicantId { get; set; }
        
        // Navigation property
        public Applicant Applicant { get; set; } = null!;
    }
}