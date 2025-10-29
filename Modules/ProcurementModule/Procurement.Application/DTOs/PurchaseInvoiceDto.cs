using System;
using System.ComponentModel.DataAnnotations;

namespace Procurement.Application.DTOs
{
    public class PurchaseInvoiceDto
    {
        public Guid Id { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}