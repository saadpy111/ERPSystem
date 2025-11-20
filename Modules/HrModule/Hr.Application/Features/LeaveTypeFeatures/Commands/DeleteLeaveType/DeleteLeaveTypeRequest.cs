using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Commands.DeleteLeaveType
{
    public class DeleteLeaveTypeRequest : IRequest<DeleteLeaveTypeResponse>
    {
        public int Id { get; set; }
    }
}