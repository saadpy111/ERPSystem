using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeAttendanceRecords
{
    public class GetEmployeeAttendanceRecordsRequest : IRequest<GetEmployeeAttendanceRecordsResponse>
    {
        public int EmployeeId { get; set; }
    }
}
