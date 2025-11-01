using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.DeleteLeaveRequest
{
    public class DeleteLeaveRequestRequest : IRequest<DeleteLeaveRequestResponse>
    {
        public int Id { get; set; }
    }
}
