using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.UpdateAttendanceRecord
{
    public class UpdateAttendanceRecordRequest : IRequest<UpdateAttendanceRecordResponse>
    {
        public int RecordId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int DelayMinutes { get; set; }
    }
}
