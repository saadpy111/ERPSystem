using MediatR;

namespace Events.WebsiteEvents
{
    /// <summary>
    /// Event raised when stock levels change in the Inventory module.
    /// Website module listens to update WebsiteProduct.IsAvailable.
    /// </summary>
    public class InventoryStockChangedEvent : INotification
    {
        /// <summary>
        /// Product ID in the Inventory module.
        /// </summary>
        public Guid InventoryProductId { get; set; }

        /// <summary>
        /// New stock quantity.
        /// </summary>
        public int NewQuantity { get; set; }

        /// <summary>
        /// Whether stock is available (quantity > 0).
        /// </summary>
        public bool IsAvailable => NewQuantity > 0;

        /// <summary>
        /// Tenant ID for multi-tenant filtering.
        /// </summary>
        public string TenantId { get; set; } = string.Empty;
    }
}
