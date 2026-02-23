namespace Website.Domain.Entities
{
    /// <summary>
    /// Join table for many-to-many relationship between Offer and WebsiteCategory.
    /// Any product whose <see cref="WebsiteProduct.CategoryId"/> matches
    /// <see cref="CategoryId"/> is eligible for this offer.
    /// </summary>
    public class OfferCategory : BaseEntity
    {
        /// <summary>
        /// The offer this entry belongs to.
        /// </summary>
        public Guid OfferId { get; set; }
        public Offer Offer { get; set; } = null!;

        /// <summary>
        /// The website category covered by this offer.
        /// </summary>
        public Guid CategoryId { get; set; }
        public WebsiteCategory Category { get; set; } = null!;
    }
}
