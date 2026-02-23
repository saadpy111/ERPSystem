namespace Website.Domain.Entities
{
    /// <summary>
    /// Records a specific instance of a coupon being used in an order.
    /// Used for enforcing usage limits per user and per coupon.
    /// </summary>
    public class CouponUsage : BaseEntity
    {
        /// <summary>
        /// The coupon that was used.
        /// </summary>
        public Guid CouponId { get; set; }
        public Coupon Coupon { get; set; } = null!;

        /// <summary>
        /// The user who used the coupon.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// The order to which the coupon was applied.
        /// </summary>
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        /// <summary>
        /// Exact timestamp of usage.
        /// </summary>
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;
    }
}
