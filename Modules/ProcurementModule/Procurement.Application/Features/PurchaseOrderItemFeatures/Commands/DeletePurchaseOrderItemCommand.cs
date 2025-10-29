using MediatR;
using System;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Commands
{
    public class DeletePurchaseOrderItemCommandRequest : IRequest<DeletePurchaseOrderItemCommandResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class DeletePurchaseOrderItemCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}