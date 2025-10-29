using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Queries
{
    public class GetPurchaseOrderItemByIdQueryHandler : IRequestHandler<GetPurchaseOrderItemByIdQueryRequest, GetPurchaseOrderItemByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetPurchaseOrderItemByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetPurchaseOrderItemByIdQueryResponse> Handle(GetPurchaseOrderItemByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.PurchaseOrderItemRepository.GetByIdAsync(request.Id);
            
            if (item == null)
            {
                return new GetPurchaseOrderItemByIdQueryResponse
                {
                    Success = false,
                    Message = "Purchase order item not found"
                };
            }
            
            // Map to DTO
            var itemDto = new PurchaseOrderItemDto
            {
                Id = item.Id,
                PurchaseOrderId = item.PurchaseOrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Total = item.Total,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            };
            
            return new GetPurchaseOrderItemByIdQueryResponse
            {
                Success = true,
                Item = itemDto
            };
        }
    }
}