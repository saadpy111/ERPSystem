using Hr.Domain.Enums;

namespace Hr.Application.DTOs
{
    public class LeaveRequestDto
    {
        public int RequestId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public LeaveRequestStatus Status { get; set; }
        public string? Notes { get; set; }
    }
}