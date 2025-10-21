using Inventory.Domain.Enums;

namespace Inventory.Application.Dtos.ProductDtos
{
    public class GetProductImageDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool IsPrimary { get; set; }
        public string? ThumbnailUrl { get; set; }
        public ProductImageType ImageType { get; set; }
        public int DisplayOrder { get; set; }
    }
}