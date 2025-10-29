using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Procurement.Domain.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        [Required]
        public Guid VendorId { get; set; }
        
        public Vendor Vendor { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime ExpectedDeliveryDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
        public ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();
        public ICollection<PurchaseInvoice> Invoices { get; set; } = new List<PurchaseInvoice>();
    }
}