using Hr.Application.DTOs;

namespace Hr.Application.Features.AttendanceRecordFeatures.CreateAttendanceRecord
{
    public class CreateAttendanceRecordResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public AttendanceRecordDto? AttendanceRecord { get; set; }
    }
}
