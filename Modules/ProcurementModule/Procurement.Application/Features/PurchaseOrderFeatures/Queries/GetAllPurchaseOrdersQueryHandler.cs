using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Queries
{
    public class GetAllPurchaseOrdersQueryHandler : IRequestHandler<GetAllPurchaseOrdersQueryRequest, GetAllPurchaseOrdersQueryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public GetAllPurchaseOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<GetAllPurchaseOrdersQueryResponse> Handle(GetAllPurchaseOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var purchaseOrders = await _unitOfWork.PurchaseOrderRepository.GetAllAsync();
            
            var purchaseOrderDtos = new List<PurchaseOrderDto>();
            
            foreach (var purchaseOrder in purchaseOrders)
            {
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
                
                purchaseOrderDtos.Add(purchaseOrderDto);
            }
            
            return new GetAllPurchaseOrdersQueryResponse
            {
                Success = true,
                PurchaseOrders = purchaseOrderDtos
            };
        }
    }
}