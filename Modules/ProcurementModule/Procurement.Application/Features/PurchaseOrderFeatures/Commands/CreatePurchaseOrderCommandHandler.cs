using MediatR;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Application.DTOs;
using Procurement.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Procurement.Application.Features.PurchaseOrderFeatures.Commands
{
    public class CreatePurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommandRequest, CreatePurchaseOrderCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CreatePurchaseOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<CreatePurchaseOrderCommandResponse> Handle(CreatePurchaseOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var purchaseOrder = new PurchaseOrder
            {
                VendorId = request.PurchaseOrder.VendorId,
                ExpectedDeliveryDate = request.PurchaseOrder.ExpectedDeliveryDate,
                CreatedBy = request.PurchaseOrder.CreatedBy,
                Status = "Draft",
                TotalAmount = request.PurchaseOrder.Items.Sum(i => i.Quantity * i.UnitPrice)
            };
            
            // Create purchase order items
            foreach (var itemDto in request.PurchaseOrder.Items)
            {
                var item = new PurchaseOrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice
                };
                
                purchaseOrder.Items.Add(item);
            }
            
            await _unitOfWork.PurchaseOrderRepository.AddAsync(purchaseOrder);
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
                CreatedAt = purchaseOrder.CreatedAt
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
                CreatedAt = item.CreatedAt
            }).ToList();
            
            return new CreatePurchaseOrderCommandResponse
            {
                Success = true,
                Message = "Purchase order created successfully",
                PurchaseOrder = purchaseOrderDto
            };
        }
    }
}