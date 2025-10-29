using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Queries
{
    public class GetPurchaseOrderByIdQueryHandler : IRequestHandler<GetPurchaseOrderByIdQueryRequest, GetPurchaseOrderByIdQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetPurchaseOrderByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetPurchaseOrderByIdQueryResponse> Handle(GetPurchaseOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _unitOfWork.PurchaseOrderRepository.GetByIdAsync(request.Id);
            
            if (purchaseOrder == null)
            {
                return new GetPurchaseOrderByIdQueryResponse
                {
                    Success = false,
                    Message = "Purchase order not found"
                };
            }
            
            // Get vendor name for DTO
            var vendor = await _unitOfWork.VendorRepository.GetByIdAsync(purchaseOrder.VendorId);
            
            // Map to DTO
            var purchaseOrderDto = new PurchaseOrderDto
            {
                Id = purchaseOrder.Id,
                VendorId = purchaseOrder.VendorId,
                VendorName = vendor?.Name ?? string.Empty,
                OrderDate = purchaseOrder.OrderDate,
                ExpectedDeliveryDate = purchaseOrder.ExpectedDeliveryDate,
                Status = purchaseOrder.Status,
                TotalAmount = purchaseOrder.TotalAmount,
                CreatedBy = purchaseOrder.CreatedBy,
                CreatedAt = purchaseOrder.CreatedAt,
                UpdatedAt = purchaseOrder.UpdatedAt
            };
            
            // Map purchase order items
            purchaseOrderDto.Items = purchaseOrder.Items.Select(item => new PurchaseOrderItemDto
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
            
            return new GetPurchaseOrderByIdQueryResponse
            {
                Success = true,
                PurchaseOrder = purchaseOrderDto
            };
        }
    }
}