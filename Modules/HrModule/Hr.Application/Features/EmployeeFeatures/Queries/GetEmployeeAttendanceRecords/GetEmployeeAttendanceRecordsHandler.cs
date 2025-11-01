using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeAttendanceRecords
{
    public class GetEmployeeAttendanceRecordsHandler : IRequestHandler<GetEmployeeAttendanceRecordsRequest, GetEmployeeAttendanceRecordsResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAttendanceRecordRepository _attendanceRecordRepository;

        public GetEmployeeAttendanceRecordsHandler(
            IEmployeeRepository employeeRepository,
            IAttendanceRecordRepository attendanceRecordRepository)
        {
            _employeeRepository = employeeRepository;
            _attendanceRecordRepository = attendanceRecordRepository;
        }

        public async Task<GetEmployeeAttendanceRecordsResponse> Handle(GetEmployeeAttendanceRecordsRequest request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                return new GetEmployeeAttendanceRecordsResponse
                {
                    EmployeeId = request.EmployeeId,
                    FullName = string.Empty,
                    AttendanceRecords = new List<AttendanceRecordDto>()
                };
            }

            var attendanceRecords = await _attendanceRecordRepository.GetByEmployeeIdAsync(request.EmployeeId);

            var attendanceDtos = attendanceRecords
                .OrderByDescending(a => a.Date)
                .Select(a => new AttendanceRecordDto
                {
                    RecordId = a.RecordId,
                    EmployeeId = a.EmployeeId,
                    Date = a.Date,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    DelayMinutes = a.DelayMinutes
                });

            return new GetEmployeeAttendanceRecordsResponse
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                AttendanceRecords = attendanceDtos
            };
        }
    }
}
