using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Commands
{
    public class UpdateGoodsReceiptCommandHandler : IRequestHandler<UpdateGoodsReceiptCommandRequest, UpdateGoodsReceiptCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdateGoodsReceiptCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<UpdateGoodsReceiptCommandResponse> Handle(UpdateGoodsReceiptCommandRequest request, CancellationToken cancellationToken)
        {
            var goodsReceipt = await _unitOfWork.GoodsReceiptRepository.GetByIdAsync(request.Id);
            
            if (goodsReceipt == null)
            {
                return new UpdateGoodsReceiptCommandResponse
                {
                    Success = false,
                    Message = "Goods receipt not found"
                };
            }
            
            // Update goods receipt properties
            if (request.ReceivedDate.HasValue)
                goodsReceipt.ReceivedDate = request.ReceivedDate.Value;
                
                goodsReceipt.ReceivedBy = request.ReceivedBy;
                

                
            if (request.Remarks != null)
                goodsReceipt.Remarks = request.Remarks;
                
            
            _unitOfWork.GoodsReceiptRepository.Update(goodsReceipt);
            await _unitOfWork.SaveChangesAsync();
            
            return new UpdateGoodsReceiptCommandResponse
            {
                Success = true,
                Message = "Goods receipt updated successfully"
            };
        }
    }
}