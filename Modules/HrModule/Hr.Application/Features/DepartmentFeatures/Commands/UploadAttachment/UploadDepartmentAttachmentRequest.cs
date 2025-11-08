using MediatR;
using Microsoft.AspNetCore.Http;

namespace Hr.Application.Features.DepartmentFeatures.Commands.UploadAttachment
{
    public class UploadDepartmentAttachmentRequest : IRequest<UploadDepartmentAttachmentResponse>
    {
        public int DepartmentId { get; set; }
        public IFormFile AttachmentFile { get; set; } = null!;
        public string? Description { get; set; }
    }
}