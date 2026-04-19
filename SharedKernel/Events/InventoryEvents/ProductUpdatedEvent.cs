using MediatR;
using System;

namespace Events.InventoryEvents
{
    public class ProductUpdatedEvent : INotification
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string? Sku { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsActive { get; set; }
        public string? ProductBarcode { get; set; }
        public string UnitOfMeasure { get; set; }
        public string CategoryName { get; set; }
    }
}
