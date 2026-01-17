using Website.Domain.Enums;
using Website.Domain.ValueObjects;

namespace Website.Domain.Entities
{
    /// <summary>
    /// Validated purchase record.
    /// Created when customer completes checkout.
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Human-readable order number.
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// User who placed the order.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Current status in fulfillment lifecycle.
        /// </summary>
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        /// <summary>
        /// Total amount for the order.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// How the customer is paying.
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CreditCard;

        /// <summary>
        /// Shipping address (stored as owned entity).
        /// </summary>
        public ShippingAddress ShippingAddress { get; set; } = new();

        /// <summary>
        /// Customer notes or special instructions.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// When the order was placed.
        /// </summary>
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
