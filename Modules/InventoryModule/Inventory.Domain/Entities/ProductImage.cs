using System;
    using Inventory.Domain.Enums;
namespace Inventory.Domain.Entities
{

    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public bool IsPrimary { get; set; }
        //public string? ThumbnailUrl { get; set; }
        //public ProductImageType ImageType { get; set; }
        public int DisplayOrder { get; set; }

        // Foreign key relationship with Product
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}