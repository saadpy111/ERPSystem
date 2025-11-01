using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class Job : BaseEntity
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public WorkType? WorkType { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Open;

        public DateTime PublishedDate { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Applicant> Applicants { get; set; } = new List<Applicant>();
    }
}
