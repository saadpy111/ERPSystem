using System;

namespace Inventory.Application.Dtos.ProductDtos
{
    public class CreateStockQuantForProductDto
    {
        public Guid LocationId { get; set; }
        public int Quantity { get; set; }
    }
}