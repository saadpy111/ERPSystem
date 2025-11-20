using Hr.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Hr.Domain.Entities
{
    public class LeaveType : BaseEntity
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string LeaveTypeName { get; set; } = string.Empty;

        public int DurationDays { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
        
        public LeaveTypeStatus Status { get; set; } = LeaveTypeStatus.NoPaid;
    }
}