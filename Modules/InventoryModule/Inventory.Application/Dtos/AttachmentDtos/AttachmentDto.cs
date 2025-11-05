using System;

namespace Inventory.Application.Dtos.AttachmentDtos
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }
        public string? Description { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
