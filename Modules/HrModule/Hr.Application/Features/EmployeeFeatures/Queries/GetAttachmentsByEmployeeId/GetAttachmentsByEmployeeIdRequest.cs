using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.Queries.GetAttachmentsByEmployeeId
{
    public class GetAttachmentsByEmployeeIdRequest : IRequest<GetAttachmentsByEmployeeIdResponse>
    {
        public int EmployeeId { get; set; }
    }
}