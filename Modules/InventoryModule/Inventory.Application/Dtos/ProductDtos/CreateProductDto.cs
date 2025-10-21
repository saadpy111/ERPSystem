using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Inventory.Application.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string? Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public List<ProductImageDto>? Images { get; set; }


        // FK: ProductCategory
        public Guid CategoryId { get; set; }
        public List<ProductAttributeValueDto>? AttributeValues { get; set; }
    }
}