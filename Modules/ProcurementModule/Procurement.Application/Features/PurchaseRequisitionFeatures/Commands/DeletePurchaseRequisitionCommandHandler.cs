using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Commands
{
    public class DeletePurchaseRequisitionCommandHandler : IRequestHandler<DeletePurchaseRequisitionCommandRequest, DeletePurchaseRequisitionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DeletePurchaseRequisitionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<DeletePurchaseRequisitionCommandResponse> Handle(DeletePurchaseRequisitionCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseRequisition = await _unitOfWork.PurchaseRequisitionRepository.GetByIdAsync(request.Id);
            
            if (purchaseRequisition == null)
            {
                return new DeletePurchaseRequisitionCommandResponse
                {
                    Success = false,
                    Message = "Purchase requisition not found"
                };
            }
            
            _unitOfWork.PurchaseRequisitionRepository.Delete(purchaseRequisition);
            await _unitOfWork.SaveChangesAsync();
            
            return new DeletePurchaseRequisitionCommandResponse
            {
                Success = true,
                Message = "Purchase requisition deleted successfully"
            };
        }
    }
}