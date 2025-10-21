using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Principal;

namespace Inventory.Application.Dtos.ProductDtos
{
    public class ProductImageDto
    {
        public Guid? Id { get; set; }
        public IFormFile Image { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Description { get; set; }
        public bool IsPrimary { get; set; }
        public ProductImageType ImageType { get; set; }
        public int DisplayOrder { get; set; }
    }
    public static class ProductImageDtoExtentions
    {
        public static ProductImage ToEntity(this ProductImageDto dto)
        {
            return new ProductImage()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                ImageType = dto.ImageType,
                Description = dto.Description,
                DisplayOrder = dto.DisplayOrder,
                ImageUrl = dto.ThumbnailUrl,
                IsPrimary = dto.IsPrimary,
                ThumbnailUrl = dto.ThumbnailUrl
            };
        }
    }
}