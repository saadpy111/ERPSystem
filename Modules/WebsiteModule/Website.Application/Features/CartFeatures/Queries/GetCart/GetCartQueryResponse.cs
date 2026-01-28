namespace Website.Application.Features.CartFeatures.Queries.GetCart
{
    public class GetCartQueryResponse
    {
        public CartDto? Cart { get; set; }
    }

    public class CartDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new();
        
        /// <summary>
        /// Subtotal before any discounts.
        /// </summary>
        public decimal Subtotal { get; set; }
        
        /// <summary>
        /// Estimated total discount from active offers (preview only).
        /// </summary>
        public decimal EstimatedDiscountTotal { get; set; }
        
        /// <summary>
        /// Estimated final total after discounts (preview only).
        /// </summary>
        public decimal EstimatedTotal { get; set; }
    }

    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public bool IsAvailable { get; set; }
        
        /// <summary>
        /// Estimated discount per item from active offers (preview only).
        /// </summary>
        public decimal EstimatedDiscount { get; set; }
        
        /// <summary>
        /// Estimated final price after discount (preview only).
        /// </summary>
        public decimal EstimatedFinalPrice { get; set; }
        
        /// <summary>
        /// Name of the applied offer (for display only).
        /// </summary>
        public string? AppliedOfferName { get; set; }
    }
}
