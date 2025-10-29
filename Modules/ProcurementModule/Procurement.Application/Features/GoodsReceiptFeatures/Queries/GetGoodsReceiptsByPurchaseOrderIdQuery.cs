using MediatR;
using Procurement.Application.DTOs;
using System;
using System.Collections.Generic;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Queries
{
    public class GetGoodsReceiptsByPurchaseOrderIdQueryRequest : IRequest<GetGoodsReceiptsByPurchaseOrderIdQueryResponse>
    {
        public Guid PurchaseOrderId { get; set; }
    }
    
    public class GetGoodsReceiptsByPurchaseOrderIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<GoodsReceiptDto> GoodsReceipts { get; set; } = new List<GoodsReceiptDto>();
    }
}