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

        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

        // New fields
        public Gender Gender { get; set; }
        
        [StringLength(500)]
        public string? Address { get; set; }
        
        [StringLength(500)]
        public string? ImageUrl { get; set; }
        
        
        // Self-referencing relationship for manager
        public int? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();

        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<PayrollRecord> PayrollRecords { get; set; } = new List<PayrollRecord>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<EmployeeContract> Contracts { get; set; } = new List<EmployeeContract>();
    }
}
