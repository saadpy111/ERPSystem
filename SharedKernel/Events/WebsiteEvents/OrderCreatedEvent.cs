using MediatR;

namespace Events.WebsiteEvents
{
    /// <summary>
    /// Event raised when an order is created in the Website module.
    /// Inventory module listens to reserve stock.
    /// </summary>
    public class OrderCreatedEvent : INotification
    {
        /// <summary>
        /// Order ID in the Website module.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Order number for reference.
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// Items in the order with quantities.
        /// </summary>
        public List<OrderItemInfo> Items { get; set; } = new();

        /// <summary>
        /// Tenant ID for multi-tenant filtering.
        /// </summary>
        public string TenantId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Order item information for stock reservation.
    /// </summary>
    public class OrderItemInfo
    {
        public Guid InventoryProductId { get; set; }
        public int Quantity { get; set; }
    }
}
