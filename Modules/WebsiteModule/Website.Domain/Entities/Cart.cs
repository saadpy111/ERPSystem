namespace Website.Domain.Entities
{
    /// <summary>
    /// Shopping cart for a customer session.
    /// Adding to cart does NOT reserve stock.
    /// </summary>
    public class Cart : BaseEntity
    {
        /// <summary>
        /// User who owns this cart (from Identity module).
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Session identifier for anonymous carts.
        /// </summary>
        public string? SessionId { get; set; }

        /// <summary>
        /// Whether the cart has been checked out.
        /// </summary>
        public bool IsCheckedOut { get; set; } = false;

        // Navigation properties
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        /// <summary>
        /// Calculated total of all items in cart.
        /// </summary>
        public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
    }
}
