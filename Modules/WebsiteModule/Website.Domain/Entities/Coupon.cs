using Website.Domain.Enums;

namespace Website.Domain.Entities
{
    /// <summary>
    /// Represents a discount coupon that can be applied to an entire order.
    /// Supports percentage or fixed discounts, usage limits, and combination rules.
    /// </summary>
    public class Coupon : BaseEntity
    {
        /// <summary>
        /// Unique code to be entered by the user (e.g., WELCOME20).
        /// Indexed for fast lookup.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Friendly name for internal display.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of discount (Percentage or Fixed).
        /// </summary>
        public DiscountType DiscountType { get; set; } = DiscountType.Percentage;

        /// <summary>
        /// Discount value (e.g., 20 for 20% or 10.50 for $10.50 off).
        /// </summary>
        public decimal DiscountValue { get; set; }

        /// <summary>
        /// When the coupon becomes valid.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// When the coupon expires.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Manual override to disable a coupon.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// If true, applies on top of line-level automatic offers.
        /// If false, offers are ignored if this coupon is used.
        /// </summary>
        public bool CanBeCombinedWithOffers { get; set; } = false;

        /// <summary>
        /// Total number of times this coupon can be used across all users.
        /// </summary>
        public int? UsageLimit { get; set; }

        /// <summary>
        /// Number of times a specific user can use this coupon.
        /// </summary>
        public int? UsagePerUserLimit { get; set; }

        /// <summary>
        /// The minimum subtotal required to apply this coupon.
        /// </summary>
        public decimal? MinimumOrderAmount { get; set; }

        // Navigation properties
        public ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
    }
}
