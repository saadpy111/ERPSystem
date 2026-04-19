using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Inventory.Application.Dtos.AttachmentDtos;
using Inventory.Domain.Enums;

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

        // POS & weighted product support
        public ProductType ProductType { get; set; } = ProductType.FinishedGood;
        public bool IsSellableInPOS { get; set; } = true;
        public bool IsWeighted { get; set; }
        public decimal? PricePerKg { get; set; }
        public decimal? DefaultTareWeight { get; set; }

        // Legacy single barcode
        public string? ProductBarcode { get; set; }
        public string? MainSupplierName { get; set; }
        public decimal? Tax { get; set; }
        public int? OrderLimit { get; set; }
        public CreateStockQuantForProductDto? InitialStockQuant { get; set; }


        // FK: ProductCategory
        public Guid CategoryId { get; set; }
        public List<ProductAttributeValueDto>? AttributeValues { get; set; }
        public List<CreateAttachmentDto>? Attachments { get; set; }
    }
}