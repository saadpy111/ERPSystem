using MediatR;
using Procurement.Application.DTOs;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Commands
{
    public class CreatePurchaseRequisitionCommandRequest : IRequest<CreatePurchaseRequisitionCommandResponse>
    {
        public CreatePurchaseRequisitionDto PurchaseRequisition { get; set; } = null!;
    }
    
    public class CreatePurchaseRequisitionCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PurchaseRequisitionDto? PurchaseRequisition { get; set; }
    }
}