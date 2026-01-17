namespace Website.Domain.Entities
{
    /// <summary>
    /// Line item in an order.
    /// </summary>
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductId { get; set; }
        public WebsiteProduct Product { get; set; } = null!;

        /// <summary>
        /// Snapshot of product name at time of order.
        /// </summary>
        public string ProductNameSnapshot { get; set; } = string.Empty;

        /// <summary>
        /// Number of units ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price at time of order.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Subtotal for this line item.
        /// </summary>
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
