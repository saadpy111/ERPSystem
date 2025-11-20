using System;
using System.ComponentModel.DataAnnotations;
using Hr.Domain.Enums;

namespace Hr.Domain.Entities
{
    public class LeaveRequest : BaseEntity
    {
        [Key]
        public int RequestId { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        [Required]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int DurationDays { get; set; }

        public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;
        
        [StringLength(1000)]
        public string? Notes { get; set; }
    }
}