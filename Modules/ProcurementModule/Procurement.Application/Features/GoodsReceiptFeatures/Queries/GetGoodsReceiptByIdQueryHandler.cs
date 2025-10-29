using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Queries
{
    public class GetGoodsReceiptByIdQueryHandler : IRequestHandler<GetGoodsReceiptByIdQueryRequest, GetGoodsReceiptByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetGoodsReceiptByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetGoodsReceiptByIdQueryResponse> Handle(GetGoodsReceiptByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var goodsReceipt = await _unitOfWork.GoodsReceiptRepository.GetByIdAsync(request.Id);
            
            if (goodsReceipt == null)
            {
                return new GetGoodsReceiptByIdQueryResponse
                {
                    Success = false,
                    Message = "Goods receipt not found"
                };
            }
            
            // Map to DTO
            var goodsReceiptDto = new GoodsReceiptDto
            {
                Id = goodsReceipt.Id,
                PurchaseOrderId = goodsReceipt.PurchaseOrderId,
                ReceivedDate = goodsReceipt.ReceivedDate,
                ReceivedBy = goodsReceipt.ReceivedBy.Value,
                Remarks = goodsReceipt.Remarks,
                CreatedAt = goodsReceipt.CreatedAt,
                UpdatedAt = goodsReceipt.UpdatedAt
            };
            
            return new GetGoodsReceiptByIdQueryResponse
            {
                Success = true,
                GoodsReceipt = goodsReceiptDto
            };
        }
    }
}