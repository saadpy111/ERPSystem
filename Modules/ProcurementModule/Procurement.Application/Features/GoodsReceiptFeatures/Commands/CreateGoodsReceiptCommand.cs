using MediatR;
using Procurement.Application.DTOs;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Commands
{
    public class CreateGoodsReceiptCommandRequest : IRequest<CreateGoodsReceiptCommandResponse>
    {
        public CreateGoodsReceiptDto GoodsReceipt { get; set; } = null!;
    }
    
    public class CreateGoodsReceiptCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public GoodsReceiptDto? GoodsReceipt { get; set; }
    }
}