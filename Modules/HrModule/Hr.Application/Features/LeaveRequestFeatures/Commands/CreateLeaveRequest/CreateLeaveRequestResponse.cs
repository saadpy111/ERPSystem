using Hr.Application.DTOs;

namespace Hr.Application.Features.LeaveRequestFeatures.CreateLeaveRequest
{
    public class CreateLeaveRequestResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public LeaveRequestDto? LeaveRequest { get; set; }
    }
}
