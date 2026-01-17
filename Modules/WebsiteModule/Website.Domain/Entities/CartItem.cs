namespace Website.Domain.Entities
{
    /// <summary>
    /// Item in a shopping cart.
    /// </summary>
    public class CartItem : BaseEntity
    {
        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        public Guid ProductId { get; set; }
        public WebsiteProduct Product { get; set; } = null!;

        /// <summary>
        /// Number of units.
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Unit price at time of adding to cart.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Subtotal for this item.
        /// </summary>
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
