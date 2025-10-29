using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseRequisitionFeatures.Commands
{
    public class UpdatePurchaseRequisitionCommandHandler : IRequestHandler<UpdatePurchaseRequisitionCommandRequest, UpdatePurchaseRequisitionCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdatePurchaseRequisitionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<UpdatePurchaseRequisitionCommandResponse> Handle(UpdatePurchaseRequisitionCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseRequisition = await _unitOfWork.PurchaseRequisitionRepository.GetByIdAsync(request.Id);
            
            if (purchaseRequisition == null)
            {
                return new UpdatePurchaseRequisitionCommandResponse
                {
                    Success = false,
                    Message = "Purchase requisition not found"
                };
            }
            
            // Update purchase requisition properties
            if (!string.IsNullOrEmpty(request.RequestedBy))
                purchaseRequisition.RequestedBy = request.RequestedBy;
                
            if (request.RequestDate.HasValue)
                purchaseRequisition.RequestDate = request.RequestDate.Value;
                
            if (!string.IsNullOrEmpty(request.Status))
                purchaseRequisition.Status = request.Status;
                
            if (request.Notes != null)
                purchaseRequisition.Notes = request.Notes;
                
            purchaseRequisition.UpdatedAt = System.DateTime.UtcNow;
            
            _unitOfWork.PurchaseRequisitionRepository.Update(purchaseRequisition);
            await _unitOfWork.SaveChangesAsync();
            
            return new UpdatePurchaseRequisitionCommandResponse
            {
                Success = true,
                Message = "Purchase requisition updated successfully"
            };
        }
    }
}