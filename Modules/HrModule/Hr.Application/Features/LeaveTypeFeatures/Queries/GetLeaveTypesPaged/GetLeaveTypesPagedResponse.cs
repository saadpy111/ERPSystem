using Hr.Application.DTOs;
using Hr.Application.Pagination;

namespace Hr.Application.Features.LeaveTypeFeatures.Queries.GetLeaveTypesPaged
{
    public class GetLeaveTypesPagedResponse
    {
        public PagedResult<LeaveTypeDto> PagedResult { get; set; } = new PagedResult<LeaveTypeDto>();
    }
}