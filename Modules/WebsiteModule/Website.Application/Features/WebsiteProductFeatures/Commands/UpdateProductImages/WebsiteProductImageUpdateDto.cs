using Microsoft.AspNetCore.Http;

namespace Website.Application.Features.WebsiteProductFeatures.Commands.UpdateProductImages
{
    public class WebsiteProductImageUpdateDto
    {
        public Guid? Id { get; set; }
        public IFormFile? Image { get; set; }
        public string? AltText { get; set; }
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDeleted { get; set; }
    }
}
