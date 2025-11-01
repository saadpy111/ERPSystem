using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeLeaveRequests
{
    public class GetEmployeeLeaveRequestsResponse
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public IEnumerable<LeaveRequestDto> LeaveRequests { get; set; } = new List<LeaveRequestDto>();
    }
}
