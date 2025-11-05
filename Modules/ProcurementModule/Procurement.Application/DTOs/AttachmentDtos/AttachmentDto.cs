using System;

namespace Procurement.Application.DTOs.AttachmentDtos
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}