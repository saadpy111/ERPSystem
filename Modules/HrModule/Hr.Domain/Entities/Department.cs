using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hr.Domain.Entities
{
    public class Department : BaseEntity
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
