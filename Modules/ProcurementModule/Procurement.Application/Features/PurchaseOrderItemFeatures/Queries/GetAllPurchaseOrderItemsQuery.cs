using MediatR;
using Procurement.Application.DTOs;
using System.Collections.Generic;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Queries
{
    public class GetAllPurchaseOrderItemsQueryRequest : IRequest<GetAllPurchaseOrderItemsQueryResponse>
    {
    }
    
    public class GetAllPurchaseOrderItemsQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PurchaseOrderItemDto> Items { get; set; } = new List<PurchaseOrderItemDto>();
    }
}