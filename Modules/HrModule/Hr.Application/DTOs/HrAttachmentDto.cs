using System;

namespace Hr.Application.DTOs
{
    public class HrAttachmentDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}