using MediatR;
using Procurement.Application.DTOs;
using System;
using System.Collections.Generic;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Queries
{
    public class GetPurchaseOrderItemsByPurchaseOrderIdQueryRequest : IRequest<GetPurchaseOrderItemsByPurchaseOrderIdQueryResponse>
    {
        public Guid PurchaseOrderId { get; set; }
    }
    
    public class GetPurchaseOrderItemsByPurchaseOrderIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PurchaseOrderItemDto> Items { get; set; } = new List<PurchaseOrderItemDto>();
    }
}