using MediatR;
using Procurement.Application.DTOs;
using System;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Queries
{
    public class GetGoodsReceiptByIdQueryRequest : IRequest<GetGoodsReceiptByIdQueryResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class GetGoodsReceiptByIdQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public GoodsReceiptDto? GoodsReceipt { get; set; }
    }
}