using MediatR;
using System;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Commands
{
    public class UpdatePurchaseRequisitionCommandRequest : IRequest<UpdatePurchaseRequisitionCommandResponse>
    {
        public Guid Id { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }
    
    public class UpdatePurchaseRequisitionCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}