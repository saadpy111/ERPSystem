using Microsoft.AspNetCore.Http;

namespace Inventory.Application.Dtos.AttachmentDtos
{
    public class CreateAttachmentDto
    {
        public IFormFile File { get; set; }
        public string? Description { get; set; }
    }
}
