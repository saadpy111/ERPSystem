using System;

namespace Inventory.Application.Dtos.ProductDtos
{
    public class CreateStockQuantForProductDto
    {
        public Guid LocationId { get; set; }
        public decimal Quantity { get; set; }
    }
}