using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Commands
{
    public class UpdatePurchaseOrderItemCommandHandler : IRequestHandler<UpdatePurchaseOrderItemCommandRequest, UpdatePurchaseOrderItemCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdatePurchaseOrderItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<UpdatePurchaseOrderItemCommandResponse> Handle(UpdatePurchaseOrderItemCommandRequest request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.PurchaseOrderItemRepository.GetByIdAsync(request.Id);
            
            if (item == null)
            {
                return new UpdatePurchaseOrderItemCommandResponse
                {
                    Success = false,
                    Message = "Purchase order item not found"
                };
            }
            
            // Update item properties
            if (request.ProductId.HasValue)
                item.ProductId = request.ProductId.Value;
                
            if (request.Quantity.HasValue)
                item.Quantity = request.Quantity.Value;
                
            if (request.UnitPrice.HasValue)
                item.UnitPrice = request.UnitPrice.Value;
                
            item.UpdatedAt = System.DateTime.UtcNow;
            
            _unitOfWork.PurchaseOrderItemRepository.Update(item);
            await _unitOfWork.SaveChangesAsync();
            
            return new UpdatePurchaseOrderItemCommandResponse
            {
                Success = true,
                Message = "Purchase order item updated successfully"
            };
        }
    }
}