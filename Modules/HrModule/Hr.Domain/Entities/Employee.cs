using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class Employee : BaseEntity
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(50)]
        public string? PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        public DateTime HiringDate { get; set; }

        [Required]
        public decimal BaseSalary { get; set; }

        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
