namespace Website.Domain.Entities
{
    /// <summary>
    /// Join table for many-to-many relationship between ProductCollection and WebsiteProduct.
    /// </summary>
    public class ProductCollectionItem : BaseEntity
    {
        public Guid CollectionId { get; set; }
        public ProductCollection Collection { get; set; } = null!;

        public Guid ProductId { get; set; }
        public WebsiteProduct Product { get; set; } = null!;

        /// <summary>
        /// Display order within the collection.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
    }
}
