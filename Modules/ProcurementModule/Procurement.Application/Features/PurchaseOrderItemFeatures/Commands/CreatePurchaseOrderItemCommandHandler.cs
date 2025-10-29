using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Commands
{
    public class CreatePurchaseOrderItemCommandHandler : IRequestHandler<CreatePurchaseOrderItemCommandRequest, CreatePurchaseOrderItemCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CreatePurchaseOrderItemCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<CreatePurchaseOrderItemCommandResponse> Handle(CreatePurchaseOrderItemCommandRequest request, CancellationToken cancellationToken)
        {
            var item = new PurchaseOrderItem
            {
                PurchaseOrderId = request.PurchaseOrderId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            };
            
            await _unitOfWork.PurchaseOrderItemRepository.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();
            
            return new CreatePurchaseOrderItemCommandResponse
            {
                Success = true,
                Message = "Purchase order item created successfully",
                ItemId = item.Id
            };
        }
    }
}