using System.ComponentModel.DataAnnotations;

namespace Hr.Domain.Entities
{
    public class ApplicantEducation : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DegreeName { get; set; } = string.Empty;

        public string? Specialization { get; set; }

        public int? GraduationYear { get; set; }

        public string? Institute { get; set; }

        public string? Grade { get; set; }

        // Foreign key
        public int ApplicantId { get; set; }
        
        // Navigation property
        public Applicant Applicant { get; set; } = null!;
    }
}