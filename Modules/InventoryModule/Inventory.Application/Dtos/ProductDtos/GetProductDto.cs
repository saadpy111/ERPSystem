using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // Category name instead of Id
        public string? CategoryName { get; set; }

        // Use response-specific attribute DTO with name + value
        public List<GetProductAttributeValueDto>? AttributeValues { get; set; }

        // New: grouped by warehouse -> locations with quantities
        public List<GetProductWarehouseDto>? Warehouses { get; set; }

        public List<GetProductImageDto>? Images { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class GetProductAttributeValueDto
    {
        public string AttributeName { get; set; }
        public string Value { get; set; }
    }

    public class GetProductWarehouseDto
    {
        public string WarehouseName { get; set; }
        public List<GetProductLocationDto> Locations { get; set; }
    }

    public class GetProductLocationDto
    {
        public string LocationName { get; set; }
        public int Quantity { get; set; }
        public int ReservedQuantity { get; set; }
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
                CategoryName = entity.Category?.Name ?? string.Empty,

                // Map Attribute values to include attribute name instead of id
                AttributeValues = entity.AttributeValues?.Select(av => new GetProductAttributeValueDto
                {
                    AttributeName = av.Attribute?.Name ?? string.Empty,
                    Value = av.Value
                }).ToList(),

                // Group stock quants by warehouse and then by location, summing quantities
                Warehouses = entity.StockQuants?.Where(sq => sq.Location != null)
                    .GroupBy(sq => sq.Location.WarehouseId)
                    .Select(wg => new GetProductWarehouseDto
                    {
                        WarehouseName = wg.Select(sq => sq.Location.Warehouse?.Name).FirstOrDefault() ?? string.Empty,
                        Locations = wg.GroupBy(sq => sq.LocationId).Select(lg => new GetProductLocationDto
                        {
                            LocationName = lg.Select(sq => sq.Location?.Name).FirstOrDefault() ?? string.Empty,
                            Quantity = lg.Sum(x => x.Quantity),
                            ReservedQuantity = lg.Sum(x => x.ReservedQuantity)
                        }).ToList()
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