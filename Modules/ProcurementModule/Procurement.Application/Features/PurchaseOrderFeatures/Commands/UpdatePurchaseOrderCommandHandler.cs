using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Commands
{
    public class UpdatePurchaseOrderCommandHandler : IRequestHandler<UpdatePurchaseOrderCommandRequest, UpdatePurchaseOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UpdatePurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<UpdatePurchaseOrderCommandResponse> Handle(UpdatePurchaseOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _unitOfWork.PurchaseOrderRepository.GetByIdAsync(request.Id);
            
            if (purchaseOrder == null)
            {
                return new UpdatePurchaseOrderCommandResponse
                {
                    Success = false,
                    Message = "Purchase order not found"
                };
            }
            
            // Update purchase order properties
            if (request.VendorId.HasValue)
                purchaseOrder.VendorId = request.VendorId.Value;
                
            if (request.ExpectedDeliveryDate.HasValue)
                purchaseOrder.ExpectedDeliveryDate = request.ExpectedDeliveryDate.Value;
                
            if (!string.IsNullOrEmpty(request.CreatedBy))
                purchaseOrder.CreatedBy = request.CreatedBy;
                
            if (!string.IsNullOrEmpty(request.Status))
                purchaseOrder.Status = request.Status;
                
            purchaseOrder.UpdatedAt = System.DateTime.UtcNow;
            
            _unitOfWork.PurchaseOrderRepository.Update(purchaseOrder);
            await _unitOfWork.SaveChangesAsync();
            
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
            
            return new UpdatePurchaseOrderCommandResponse
            {
                Success = true,
                Message = "Purchase order updated successfully",
                PurchaseOrder = purchaseOrderDto
            };
        }
    }
}