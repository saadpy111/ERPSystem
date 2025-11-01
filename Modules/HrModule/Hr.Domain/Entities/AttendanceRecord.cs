using System;
using System.ComponentModel.DataAnnotations;

namespace Hr.Domain.Entities
{
    public class AttendanceRecord : BaseEntity
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public DateTime Date { get; set; }

        public DateTime? CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        public int DelayMinutes { get; set; }
    }
}
