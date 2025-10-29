using MediatR;
using System;
using System.Collections.Generic;

namespace SharedKernel.DomainEvents
{
    public class GoodsReceivedEvent : INotification
    {
        public Guid GoodsReceiptId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid LocationId { get; set; }
        public List<ReceivedItem> Items { get; set; } = new();
    }

    public class ReceivedItem
    {
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
    }
}
