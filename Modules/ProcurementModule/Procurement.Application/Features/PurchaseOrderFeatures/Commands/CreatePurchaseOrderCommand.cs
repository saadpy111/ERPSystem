using MediatR;
using Procurement.Application.DTOs;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Commands
{
    public class CreatePurchaseOrderCommandRequest : IRequest<CreatePurchaseOrderCommandResponse>
    {
        public CreatePurchaseOrderDto PurchaseOrder { get; set; } = null!;
    }
    
    public class CreatePurchaseOrderCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PurchaseOrderDto? PurchaseOrder { get; set; }
    }
}