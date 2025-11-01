using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeAttendanceRecords
{
    public class GetEmployeeAttendanceRecordsResponse
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public IEnumerable<AttendanceRecordDto> AttendanceRecords { get; set; } = new List<AttendanceRecordDto>();
    }
}
