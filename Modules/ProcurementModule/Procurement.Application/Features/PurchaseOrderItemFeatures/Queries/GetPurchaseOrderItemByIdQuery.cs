using MediatR;
using Procurement.Application.DTOs;
using System;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Queries
{
    public class GetPurchaseOrderItemByIdQueryRequest : IRequest<GetPurchaseOrderItemByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class GetPurchaseOrderItemByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PurchaseOrderItemDto? Item { get; set; }
    }
}