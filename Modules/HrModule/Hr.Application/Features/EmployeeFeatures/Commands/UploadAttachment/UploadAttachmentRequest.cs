using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.EmployeeFeatures.Commands.UploadAttachment
{
    public class UploadAttachmentRequest : IRequest<UploadAttachmentResponse>
    {
        public int EmployeeId { get; set; }
        public IFormFile AttachmentFile { get; set; } = null!;
        public string? Description { get; set; }
    }
}