using MediatR;
using System;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Commands
{
    public class CreatePurchaseOrderItemCommandRequest : IRequest<CreatePurchaseOrderItemCommandResponse>
    {
        public Guid PurchaseOrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    
    public class CreatePurchaseOrderItemCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ItemId { get; set; }
    }
}