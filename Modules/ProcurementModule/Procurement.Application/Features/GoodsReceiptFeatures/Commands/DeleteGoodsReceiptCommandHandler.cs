using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.GoodsReceiptFeatures.Commands
{
    public class DeleteGoodsReceiptCommandHandler : IRequestHandler<DeleteGoodsReceiptCommandRequest, DeleteGoodsReceiptCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DeleteGoodsReceiptCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<DeleteGoodsReceiptCommandResponse> Handle(DeleteGoodsReceiptCommandRequest request, CancellationToken cancellationToken)
        {
            var goodsReceipt = await _unitOfWork.GoodsReceiptRepository.GetByIdAsync(request.Id);
            
            if (goodsReceipt == null)
            {
                return new DeleteGoodsReceiptCommandResponse
                {
                    Success = false,
                    Message = "Goods receipt not found"
                };
            }
            
            _unitOfWork.GoodsReceiptRepository.Delete(goodsReceipt);
            await _unitOfWork.SaveChangesAsync();
            
            return new DeleteGoodsReceiptCommandResponse
            {
                Success = true,
                Message = "Goods receipt deleted successfully"
            };
        }
    }
}