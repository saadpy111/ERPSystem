using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Inventory.Application.Dtos.AttachmentDtos;

namespace Inventory.Application.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public string? Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }

        public bool IsActive { get; set; }

        // FK: ProductCategory
        public Guid CategoryId { get; set; }

        // New fields
        public string? ProductBarcode { get; set; }
        public string? MainSupplierName { get; set; }
        public decimal? Tax { get; set; }
        public int? OrderLimit { get; set; }

        public List<UpdateProductAttributeValueDto>? AttributeValues { get; set; }

        // Images: new files to add and existing images to delete
        public List<ProductImageDto>? Images { get; set; }
        public bool EditAttachments { get; set; }

        // Attachments: new files to add and existing attachments to delete
        public List<CreateAttachmentDto>? Attachments { get; set; }
    }
}