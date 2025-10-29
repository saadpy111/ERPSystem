using System;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class GoodsReceiptDto
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public Guid ReceivedBy { get; set; } = Guid.NewGuid();
        public string Status { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}