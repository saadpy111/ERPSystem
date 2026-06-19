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

        /// <summary>
        /// Snapshot of the customer's full name at time of order.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// Snapshot of the customer's phone number at time of order.
        /// </summary>
        public string CustomerPhone { get; set; } = string.Empty;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        /// <summary>
        /// Sum of all items before discounts.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Total discount applied to the order (includes offers and coupons).
        /// </summary>
        public decimal DiscountTotal { get; set; }

        /// <summary>
        /// The coupon code applied to this order, if any.
        /// </summary>
        public string? AppliedCouponCode { get; set; }

        /// <summary>
        /// Portion of the DiscountTotal contributed by the coupon.
        /// </summary>
        public decimal CouponDiscountAmount { get; set; }

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
