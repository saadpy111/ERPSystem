using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Application.DTOs;
using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeLeaveRequests
{
    public class GetEmployeeLeaveRequestsHandler : IRequestHandler<GetEmployeeLeaveRequestsRequest, GetEmployeeLeaveRequestsResponse>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        public GetEmployeeLeaveRequestsHandler(
            IEmployeeRepository employeeRepository,
            ILeaveRequestRepository leaveRequestRepository)
        {
            _employeeRepository = employeeRepository;
            _leaveRequestRepository = leaveRequestRepository;
        }

        public async Task<GetEmployeeLeaveRequestsResponse> Handle(GetEmployeeLeaveRequestsRequest request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
            {
                return new GetEmployeeLeaveRequestsResponse
                {
                    EmployeeId = request.EmployeeId,
                    FullName = string.Empty,
                    LeaveRequests = new List<LeaveRequestDto>()
                };
            }

            var leaveRequests = await _leaveRequestRepository.GetByEmployeeIdAsync(request.EmployeeId);

            var leaveDtos = leaveRequests
                .OrderByDescending(l => l.StartDate)
                .Select(l => new LeaveRequestDto
                {
                    RequestId = l.RequestId,
                    EmployeeId = l.EmployeeId,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    DurationDays = l.DurationDays,
                    Status = l.Status
                });

            return new GetEmployeeLeaveRequestsResponse
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                LeaveRequests = leaveDtos
            };
        }
    }
}
