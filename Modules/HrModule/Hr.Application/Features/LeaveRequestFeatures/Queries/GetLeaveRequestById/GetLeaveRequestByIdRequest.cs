using MediatR;

namespace Hr.Application.Features.LeaveRequestFeatures.GetLeaveRequestById
{
    public class GetLeaveRequestByIdRequest : IRequest<GetLeaveRequestByIdResponse>
    {
        public int Id { get; set; }
    }
}
