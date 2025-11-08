using Hr.Application.DTOs;

namespace Hr.Application.Features.ApplicantFeatures.Commands.UploadAttachmentApplicant
{
    public class UploadAttachmentApplicantResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ICollection<HrAttachmentDto> Attachments { get; set; } = new List<HrAttachmentDto>();
    }
}