using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.CreateAttendanceRecord
{
    public class CreateAttendanceRecordRequest : IRequest<CreateAttendanceRecordResponse>
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int DelayMinutes { get; set; }
    }
}
