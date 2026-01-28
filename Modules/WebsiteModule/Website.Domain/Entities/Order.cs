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
        public string OrderNumber { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        /// <summary>
        /// Sum of all items before discounts.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Total discount applied to the order.
        /// </summary>
        public decimal DiscountTotal { get; set; }

        /// <summary>
        /// Final amount after discounts.
        /// </summary>
        public decimal TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CreditCard;

        public ShippingAddress ShippingAddress { get; set; } = new();

        public string? Notes { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
