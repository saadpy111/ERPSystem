using System;
using System.ComponentModel.DataAnnotations;
using Hr.Domain.Entities;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class EmployeeContract : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required]
        public int JobId { get; set; }
        public Job Job { get; set; } = null!;

        [Required]
        public decimal Salary { get; set; }
        public int? ProbationPeriod { get; set; }

        public ContractType ContractType { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
    }
}