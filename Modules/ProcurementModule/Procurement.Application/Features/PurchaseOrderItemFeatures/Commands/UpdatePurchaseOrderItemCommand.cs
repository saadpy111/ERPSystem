using MediatR;
using System;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Commands
{
    public class UpdatePurchaseOrderItemCommandRequest : IRequest<UpdatePurchaseOrderItemCommandResponse>
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
    
    public class UpdatePurchaseOrderItemCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}