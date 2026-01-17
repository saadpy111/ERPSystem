namespace Website.Domain.Entities
{
    /// <summary>
    /// Join table for many-to-many relationship between Offer and WebsiteProduct.
    /// </summary>
    public class OfferProduct : BaseEntity
    {
        public Guid OfferId { get; set; }
        public Offer Offer { get; set; } = null!;

        public Guid ProductId { get; set; }
        public WebsiteProduct Product { get; set; } = null!;
    }
}
