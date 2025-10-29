using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderItemFeatures.Queries
{
    public class GetAllPurchaseOrderItemsQueryHandler : IRequestHandler<GetAllPurchaseOrderItemsQueryRequest, GetAllPurchaseOrderItemsQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetAllPurchaseOrderItemsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetAllPurchaseOrderItemsQueryResponse> Handle(GetAllPurchaseOrderItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.PurchaseOrderItemRepository.GetAllAsync();
            
            // Map to DTOs
            var itemDtos = items.Select(item => new PurchaseOrderItemDto
            {
                Id = item.Id,
                PurchaseOrderId = item.PurchaseOrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Total = item.Total,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt
            }).ToList();
            
            return new GetAllPurchaseOrderItemsQueryResponse
            {
                Success = true,
                Items = itemDtos
            };
        }
    }
}