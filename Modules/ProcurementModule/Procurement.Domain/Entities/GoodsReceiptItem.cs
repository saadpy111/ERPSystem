using System;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Domain.Entities
{
    public class GoodsReceiptItem : BaseEntity
    {
        [Required]
        public Guid GoodsReceiptId { get; set; }
        
        public GoodsReceipt GoodsReceipt { get; set; } = null!;

        [Required]
        public Guid ProductId { get; set; }
        
        [Required]
        public int ReceivedQuantity { get; set; }
        
        public decimal UnitPrice { get; set; }
        
        [MaxLength(500)]
        public string? Remarks { get; set; }
    }
}