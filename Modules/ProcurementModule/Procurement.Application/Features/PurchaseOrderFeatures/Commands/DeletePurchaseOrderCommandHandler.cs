using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Commands
{
    public class DeletePurchaseOrderCommandHandler : IRequestHandler<DeletePurchaseOrderCommandRequest, DeletePurchaseOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DeletePurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<DeletePurchaseOrderCommandResponse> Handle(DeletePurchaseOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _unitOfWork.PurchaseOrderRepository.GetByIdAsync(request.Id);
            
            if (purchaseOrder == null)
            {
                return new DeletePurchaseOrderCommandResponse
                {
                    Success = false,
                    Message = "Purchase order not found"
                };
            }
            
            _unitOfWork.PurchaseOrderRepository.Delete(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();
            
            return new DeletePurchaseOrderCommandResponse
            {
                Success = true,
                Message = "Purchase order deleted successfully"
            };
        }
    }
}