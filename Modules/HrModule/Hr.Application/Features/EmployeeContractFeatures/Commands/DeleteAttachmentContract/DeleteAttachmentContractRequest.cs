using MediatR;

namespace Hr.Application.Features.EmployeeContractFeatures.Commands.DeleteAttachmentContract
{
    public class DeleteAttachmentContractRequest : IRequest<DeleteAttachmentContractResponse>
    {
        public int AttachmentId { get; set; }
    }
}