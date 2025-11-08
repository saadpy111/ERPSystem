using MediatR;

namespace Hr.Application.Features.ApplicantFeatures.Commands.DeleteAttachmentApplicant
{
    public class DeleteAttachmentApplicantRequest : IRequest<DeleteAttachmentApplicantResponse>
    {
        public int AttachmentId { get; set; }
    }
}