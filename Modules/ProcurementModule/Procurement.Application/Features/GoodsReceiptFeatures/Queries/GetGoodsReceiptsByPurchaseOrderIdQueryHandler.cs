using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Queries
{
    public class GetGoodsReceiptsByPurchaseOrderIdQueryHandler : IRequestHandler<GetGoodsReceiptsByPurchaseOrderIdQueryRequest, GetGoodsReceiptsByPurchaseOrderIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetGoodsReceiptsByPurchaseOrderIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetGoodsReceiptsByPurchaseOrderIdQueryResponse> Handle(GetGoodsReceiptsByPurchaseOrderIdQueryRequest request, CancellationToken cancellationToken)
        {
            var goodsReceipts = await _unitOfWork.GoodsReceiptRepository.GetByPurchaseOrderIdAsync(request.PurchaseOrderId);
            
            // Map to DTOs
            var goodsReceiptDtos = goodsReceipts.Select(goodsReceipt => new GoodsReceiptDto
            {
                Id = goodsReceipt.Id,
                PurchaseOrderId = goodsReceipt.PurchaseOrderId,
                ReceivedDate = goodsReceipt.ReceivedDate,
                ReceivedBy = goodsReceipt.ReceivedBy.Value,
                Remarks = goodsReceipt.Remarks,
                CreatedAt = goodsReceipt.CreatedAt,
                UpdatedAt = goodsReceipt.UpdatedAt
            }).ToList();
            
            return new GetGoodsReceiptsByPurchaseOrderIdQueryResponse
            {
                Success = true,
                GoodsReceipts = goodsReceiptDtos
            };
        }
    }
}