using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.DeleteAttendanceRecord
{
    public class DeleteAttendanceRecordRequest : IRequest<DeleteAttendanceRecordResponse>
    {
        public int RecordId { get; set; }
    }
}
