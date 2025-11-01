using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.GetEmployeeLeaveRequests
{
    public class GetEmployeeLeaveRequestsRequest : IRequest<GetEmployeeLeaveRequestsResponse>
    {
        public int EmployeeId { get; set; }
    }
}

