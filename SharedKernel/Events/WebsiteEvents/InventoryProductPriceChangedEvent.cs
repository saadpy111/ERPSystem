using MediatR;

namespace Events.WebsiteEvents
{
    /// <summary>
    /// Event raised when a product's price changes in the Inventory module.
    /// Website module listens to update WebsiteProduct.Price.
    /// </summary>
    public class InventoryProductPriceChangedEvent : INotification
    {
        /// <summary>
        /// Product ID in the Inventory module.
        /// </summary>
        public Guid InventoryProductId { get; set; }

        /// <summary>
        /// The new sale price.
        /// </summary>
        public decimal NewPrice { get; set; }

        /// <summary>
        /// Tenant ID for multi-tenant filtering.
        /// </summary>
        public string TenantId { get; set; } = string.Empty;
    }
}
