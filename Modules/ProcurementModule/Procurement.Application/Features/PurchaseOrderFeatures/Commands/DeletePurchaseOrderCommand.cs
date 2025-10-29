using MediatR;
using System;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Commands
{
    public class DeletePurchaseOrderCommandRequest : IRequest<DeletePurchaseOrderCommandResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class DeletePurchaseOrderCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}