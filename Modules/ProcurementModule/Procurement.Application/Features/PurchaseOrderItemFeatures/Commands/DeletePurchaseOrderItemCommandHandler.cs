using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Commands
{
    public class DeletePurchaseOrderItemCommandHandler : IRequestHandler<DeletePurchaseOrderItemCommandRequest, DeletePurchaseOrderItemCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DeletePurchaseOrderItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<DeletePurchaseOrderItemCommandResponse> Handle(DeletePurchaseOrderItemCommandRequest request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.PurchaseOrderItemRepository.GetByIdAsync(request.Id);
            
            if (item == null)
            {
                return new DeletePurchaseOrderItemCommandResponse
                {
                    Success = false,
                    Message = "Purchase order item not found"
                };
            }
            
            _unitOfWork.PurchaseOrderItemRepository.Delete(item);
            await _unitOfWork.SaveChangesAsync();
            
            return new DeletePurchaseOrderItemCommandResponse
            {
                Success = true,
                Message = "Purchase order item deleted successfully"
            };
        }
    }
}