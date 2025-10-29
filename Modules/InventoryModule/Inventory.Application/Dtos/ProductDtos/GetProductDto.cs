using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Inventory.Application.Dtos.ProductDtos
{
    public class GetProductDto
    {
        public Guid Id { get; set; }
        public string? Sku { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsActive { get; set; }

        // FK: ProductCategory
        public Guid CategoryId { get; set; }

        public List<ProductAttributeValueDto>? AttributeValues { get; set; }

 
        public List<GetProductImageDto>? Images { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public static class ProductExtentions
    {
        public static GetProductDto ToDto(this Product entity)
        {
            return new GetProductDto()
            {
                Id = entity.Id,
                Sku = entity.Sku,
                Name = entity.Name,
                Description = entity.Description,
                UnitOfMeasure = entity.UnitOfMeasure,
                SalePrice = entity.SalePrice,
                CostPrice = entity.CostPrice,
                IsActive = entity.IsActive,
                CategoryId = entity.CategoryId,
                AttributeValues = entity.AttributeValues?.Select(av => new ProductAttributeValueDto
                {
                    AttributeId = av.AttributeId,
                    Value = av.Value
                }).ToList(),
                Images = entity.Images?.Select(i => new GetProductImageDto()
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    IsPrimary = i.IsPrimary,
                    Description = i.Description,
                    DisplayOrder = i.DisplayOrder,

                }).ToList(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt

            };
        }
    }
}