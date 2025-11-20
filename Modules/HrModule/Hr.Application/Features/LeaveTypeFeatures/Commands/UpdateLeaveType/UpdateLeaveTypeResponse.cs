using Hr.Application.DTOs;

namespace Hr.Application.Features.LeaveTypeFeatures.Commands.UpdateLeaveType
{
    public class UpdateLeaveTypeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public LeaveTypeDto? LeaveType { get; set; }
    }
}