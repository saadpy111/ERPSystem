using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class LeaveTypeDto
    {
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public string? Notes { get; set; }
        public LeaveTypeStatus Status { get; set; }
    }
}