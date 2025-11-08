using MediatR;

namespace Hr.Application.Features.DepartmentFeatures.Commands.DeleteAttachment
{
    public class DeleteDepartmentAttachmentRequest : IRequest<DeleteDepartmentAttachmentResponse>
    {
        public int AttachmentId { get; set; }
    }
}