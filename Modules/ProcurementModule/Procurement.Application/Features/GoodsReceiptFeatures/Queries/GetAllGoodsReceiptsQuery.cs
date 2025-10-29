using MediatR;
using Procurement.Application.DTOs;
using System.Collections.Generic;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Queries
{
    public class GetAllGoodsReceiptsQueryRequest : IRequest<GetAllGoodsReceiptsQueryResponse>
    {
    }
    
    public class GetAllGoodsReceiptsQueryResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<GoodsReceiptDto> GoodsReceipts { get; set; } = new List<GoodsReceiptDto>();
    }
}