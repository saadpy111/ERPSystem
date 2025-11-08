using Hr.Application.DTOs;

namespace Hr.Application.Features.EmployeeFeatures.Commands.UploadAttachment
{
    public class UploadAttachmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public HrAttachmentDto? Attachment { get; set; }
    }
}