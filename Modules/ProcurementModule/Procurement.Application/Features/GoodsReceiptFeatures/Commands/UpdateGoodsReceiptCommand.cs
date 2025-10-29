using MediatR;
using System;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Commands
{
    public class UpdateGoodsReceiptCommandRequest : IRequest<UpdateGoodsReceiptCommandResponse>
    {
        public Guid Id { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public Guid ReceivedBy { get; set; } = Guid.NewGuid();
        public string? Status { get; set; }
        public string? Remarks { get; set; }
    }
    
    public class UpdateGoodsReceiptCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}