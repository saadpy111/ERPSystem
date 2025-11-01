using Hr.Application.Pagination;
using Hr.Application.DTOs;

namespace Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordsPaged
{
    public class GetAttendanceRecordsPagedResponse
    {
        public PagedResult<AttendanceRecordDto> PagedResult { get; set; } = new PagedResult<AttendanceRecordDto>();
    }
}
