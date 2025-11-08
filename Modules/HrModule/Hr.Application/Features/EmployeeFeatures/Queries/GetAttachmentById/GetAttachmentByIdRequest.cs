using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.Queries.GetAttachmentById
{
    public class GetAttachmentByIdRequest : IRequest<GetAttachmentByIdResponse>
    {
        public int AttachmentId { get; set; }
    }
}