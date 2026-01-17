using Website.Domain.Enums;

namespace Website.Domain.Entities
{
    /// <summary>
    /// Discount campaign or promotional offer.
    /// Can apply to specific products via OfferProduct join table.
    /// </summary>
    public class Offer : BaseEntity
    {
        /// <summary>
        /// Offer name for display.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Marketing description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Type of discount (Percentage or Fixed).
        /// </summary>
        public DiscountType DiscountType { get; set; } = DiscountType.Percentage;

        /// <summary>
        /// Discount value (e.g., 20 for 20% or 10 for $10 off).
        /// </summary>
        public decimal DiscountValue { get; set; }

        /// <summary>
        /// When the offer becomes active.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// When the offer expires.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Whether the offer is currently active.
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<OfferProduct> OfferProducts { get; set; } = new List<OfferProduct>();
    }
}
