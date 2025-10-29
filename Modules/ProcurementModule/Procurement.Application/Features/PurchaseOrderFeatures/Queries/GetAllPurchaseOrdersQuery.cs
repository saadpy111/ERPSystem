using MediatR;
using Procurement.Application.DTOs;
using System.Collections.Generic;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Queries
{
    public class GetAllPurchaseOrdersQueryRequest : IRequest<GetAllPurchaseOrdersQueryResponse>
    {
    }
    
    public class GetAllPurchaseOrdersQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PurchaseOrderDto> PurchaseOrders { get; set; } = new List<PurchaseOrderDto>();
    }
}