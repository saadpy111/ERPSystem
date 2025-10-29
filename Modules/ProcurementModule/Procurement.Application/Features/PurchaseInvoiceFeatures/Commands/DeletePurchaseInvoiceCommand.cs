using MediatR;
using System;

namespace Procurement.Application.Features.PurchaseInvoiceFeatures.Commands
{
    public class DeletePurchaseInvoiceCommandRequest : IRequest<DeletePurchaseInvoiceCommandResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class DeletePurchaseInvoiceCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}