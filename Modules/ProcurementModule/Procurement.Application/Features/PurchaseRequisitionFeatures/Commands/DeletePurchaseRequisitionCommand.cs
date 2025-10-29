using MediatR;
using System;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Commands
{
    public class DeletePurchaseRequisitionCommandRequest : IRequest<DeletePurchaseRequisitionCommandResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class DeletePurchaseRequisitionCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}