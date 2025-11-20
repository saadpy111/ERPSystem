using MediatR;

namespace Hr.Application.Features.LeaveTypeFeatures.Queries.GetLeaveTypeById
{
    public class GetLeaveTypeByIdRequest : IRequest<GetLeaveTypeByIdResponse>
    {
        public int Id { get; set; }
    }
}