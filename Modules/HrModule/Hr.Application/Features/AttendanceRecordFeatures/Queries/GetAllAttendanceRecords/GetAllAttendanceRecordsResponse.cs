using Hr.Application.DTOs;

namespace Hr.Application.Features.AttendanceRecordFeatures.GetAllAttendanceRecords
{
    public class GetAllAttendanceRecordsResponse
    {
        public IEnumerable<AttendanceRecordDto> AttendanceRecords { get; set; } = new List<AttendanceRecordDto>();
    }
}
