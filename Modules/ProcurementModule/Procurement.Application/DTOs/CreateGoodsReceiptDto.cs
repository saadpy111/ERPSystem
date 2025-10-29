using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class CreateGoodsReceiptDto
    {
        [Required]
        public Guid PurchaseOrderId { get; set; }
        public Guid LocationId { get; set; }
        public Guid WarehouseId { get; set; }
        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;
        public Guid ReceivedBy { get; set; }
        public string? Remarks { get; set; }

        // ? ??? ??? ???????
        public List<CreateGoodsReceiptItemDto> Items { get; set; } = new();
    }

    public class CreateGoodsReceiptItemDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public decimal ReceivedQuantity { get; set; }
    }
}
