using MediatR;
using Procurement.Application.DTOs;
using System;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Commands
{
    public class UpdatePurchaseOrderCommandRequest : IRequest<UpdatePurchaseOrderCommandResponse>
    {
        public Guid Id { get; set; }
        public Guid? VendorId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? Status { get; set; }
    }
    
    public class UpdatePurchaseOrderCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PurchaseOrderDto? PurchaseOrder { get; set; }
    }
}