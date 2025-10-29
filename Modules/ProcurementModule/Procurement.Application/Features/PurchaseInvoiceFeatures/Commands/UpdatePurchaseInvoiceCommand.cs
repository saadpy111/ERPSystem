using MediatR;
using System;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Commands
{
    public class UpdatePurchaseInvoiceCommandRequest : IRequest<UpdatePurchaseInvoiceCommandResponse>
    {
        public Guid Id { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Notes { get; set; }
    }
    
    public class UpdatePurchaseInvoiceCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}