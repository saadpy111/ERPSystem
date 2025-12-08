using Events.InventoryEvents;
using MediatR;
using Report.Application.Contracts.Persistence.Repositories;
using Report.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Application.Consumers.InventoryConsumers
{
    public class NewProductCreatedConsumer : INotificationHandler<NewProductCreatedEvent>
    {
        private readonly IInventoryReportRepository _inventoryReportRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NewProductCreatedConsumer(IInventoryReportRepository inventoryReportRepository, IUnitOfWork unitOfWork)
        {
            _inventoryReportRepository = inventoryReportRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(NewProductCreatedEvent notification, CancellationToken cancellationToken)
        {
  
            // Create new inventory report entry
            var inventoryReport = new InventoryReport
            {
                ProductName = notification.Name,
                Sku = notification.Sku ?? string.Empty,
                Barcode = notification.ProductBarcode ?? string.Empty,
                CategoryName = notification.CategoryName,
                CostPrice = notification.CostPrice,
                Description = notification.Description,
                IsActive = notification.IsActive,
                MainSupplierName = notification.MainSupplierName,
                OrderLimit = notification.OrderLimit,
                Tax = notification.Tax,
                UpdatedAt = DateTime.UtcNow,
                SalePrice = notification.SalePrice,
                UnitOfMeasure = notification.UnitOfMeasure,
                UnitCost = notification.CostPrice,            
                CreatedAt = DateTime.UtcNow
            };

            await _inventoryReportRepository.AddAsync(inventoryReport);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}