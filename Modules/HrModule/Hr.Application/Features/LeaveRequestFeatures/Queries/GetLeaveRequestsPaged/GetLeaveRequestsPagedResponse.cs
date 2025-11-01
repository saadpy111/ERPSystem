using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestsPaged
{
    public class GetLeaveRequestsPagedResponse
    {
        public PagedResult<LeaveRequestDto> PagedResult { get; set; } = new PagedResult<LeaveRequestDto>();
    }
}
