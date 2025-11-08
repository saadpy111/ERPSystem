using MediatR;

namespace Hr.Application.Features.EmployeeFeatures.Commands.DeleteAttachment
{
    public class DeleteAttachmentRequest : IRequest<DeleteAttachmentResponse>
    {
        public int AttachmentId { get; set; }
    }
}