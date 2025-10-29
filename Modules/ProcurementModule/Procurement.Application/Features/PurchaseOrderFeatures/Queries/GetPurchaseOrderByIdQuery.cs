using MediatR;
using Procurement.Application.DTOs;
using System;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Queries
{
    public class GetPurchaseOrderByIdQueryRequest : IRequest<GetPurchaseOrderByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class GetPurchaseOrderByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PurchaseOrderDto? PurchaseOrder { get; set; }
    }
}