using MediatR;
using Procurement.Application.DTOs;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Commands
{
    public class CreatePurchaseInvoiceCommandRequest : IRequest<CreatePurchaseInvoiceCommandResponse>
    {
        public CreatePurchaseInvoiceDto PurchaseInvoice { get; set; } = null!;
    }
    
    public class CreatePurchaseInvoiceCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PurchaseInvoiceDto? PurchaseInvoice { get; set; }
    }
}