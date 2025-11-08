using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.ApplicantFeatures.Commands.UploadAttachmentApplicant
{
    public class UploadAttachmentApplicantRequest : IRequest<UploadAttachmentApplicantResponse>
    {
        public int ApplicantId { get; set; }
        public ICollection<IFormFile> AttachmentFiles { get; set; } = new List<IFormFile>();
    }
}