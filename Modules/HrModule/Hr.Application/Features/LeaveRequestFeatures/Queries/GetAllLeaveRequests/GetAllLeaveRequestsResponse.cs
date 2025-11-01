using Hr.Application.DTOs;

namespace Hr.Application.Features.LeaveRequestFeatures.GetAllLeaveRequests
{
    public class GetAllLeaveRequestsResponse
    {
        public IEnumerable<LeaveRequestDto> LeaveRequests { get; set; } = new List<LeaveRequestDto>();
    }
}
