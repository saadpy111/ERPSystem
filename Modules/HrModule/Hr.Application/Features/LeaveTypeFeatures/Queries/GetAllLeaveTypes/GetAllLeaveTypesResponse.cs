using Hr.Application.DTOs;
using System.Collections.Generic;

namespace Hr.Application.Features.LeaveTypeFeatures.Queries.GetAllLeaveTypes
{
    public class GetAllLeaveTypesResponse
    {
        public IEnumerable<LeaveTypeDto> LeaveTypes { get; set; } = new List<LeaveTypeDto>();
    }
}