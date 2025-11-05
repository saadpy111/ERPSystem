using Microsoft.AspNetCore.Http;

namespace Procurement.Application.DTOs.AttachmentDtos
{
    public class CreateAttachmentDto
    {
        public IFormFile File { get; set; } = null!;
        public string? Description { get; set; }
    }
}