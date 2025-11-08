using Hr.Application.DTOs;

namespace Hr.Application.Features.DepartmentFeatures.Commands.UploadAttachment
{
    public class UploadDepartmentAttachmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public HrAttachmentDto? Attachment { get; set; }
    }
}