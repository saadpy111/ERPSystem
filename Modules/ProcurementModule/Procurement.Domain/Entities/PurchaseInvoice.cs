using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Procurement.Domain.Entities
{
    public class PurchaseInvoice : BaseEntity
    {
        [Required]
        public Guid PurchaseOrderId { get; set; }
        
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; } = string.Empty;

        public DateTime? PaymentDate { get; set; }

        [MaxLength(500)]
        public string? Notes { get; set; }
    }
}