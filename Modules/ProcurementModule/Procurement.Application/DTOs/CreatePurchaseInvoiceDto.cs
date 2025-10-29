using System;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class CreatePurchaseInvoiceDto
    {
        [Required]
        public Guid PurchaseOrderId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;
        
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        
        public decimal TotalAmount { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; } = string.Empty;
        
        public DateTime? PaymentDate { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}