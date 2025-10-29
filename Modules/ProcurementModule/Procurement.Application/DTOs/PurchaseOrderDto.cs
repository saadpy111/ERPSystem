using System;
using System.Collections.Generic;

namespace Procurement.Application.DTOs
{
    public class PurchaseOrderDto
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<PurchaseOrderItemDto> Items { get; set; } = new List<PurchaseOrderItemDto>();
        public ICollection<GoodsReceiptDto> GoodsReceipts { get; set; } = new List<GoodsReceiptDto>();
        public ICollection<PurchaseInvoiceDto> Invoices { get; set; } = new List<PurchaseInvoiceDto>();
    }
}