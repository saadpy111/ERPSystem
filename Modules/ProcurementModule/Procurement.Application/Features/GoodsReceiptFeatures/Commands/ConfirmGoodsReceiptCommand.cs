using MediatR;
using System;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Commands
{
    public class ConfirmGoodsReceiptCommandRequest : IRequest<ConfirmGoodsReceiptCommandResponse>
    {
        public Guid GoodsReceiptId { get; set; }
    }
    
    public class ConfirmGoodsReceiptCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}