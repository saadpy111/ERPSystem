using MediatR;

namespace Hr.Application.Features.AttendanceRecordFeatures.GetAttendanceRecordById
{
    public class GetAttendanceRecordByIdRequest : IRequest<GetAttendanceRecordByIdResponse>
    {
        public int Id { get; set; }
    }
}
