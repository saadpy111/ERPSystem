using MediatR;
using System;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Commands
{
    public class DeleteGoodsReceiptCommandRequest : IRequest<DeleteGoodsReceiptCommandResponse>
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteGoodsReceiptCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}