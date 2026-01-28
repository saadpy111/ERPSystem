using System.ComponentModel.DataAnnotations;

namespace Website.Domain.Entities
{
    /// <summary>
    /// Line item in an order.
    /// </summary>
    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductId { get; set; }
        public WebsiteProduct Product { get; set; } = null!;

        /// <summary>
        /// Snapshot of product name at time of order.
        /// </summary>
        public string ProductNameSnapshot { get; set; } = string.Empty;

        public int Quantity { get; set; }

        /// <summary>
        /// Unit price before discount.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Total discount applied to this line item.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Final price for this line item after discount.
        /// </summary>
        public decimal FinalPrice { get; set; }

        /// <summary>
        /// Offer name applied at checkout (snapshot).
        /// </summary>
        /// 
        [MaxLength(200)]
        public string? AppliedOfferName { get; set; }

        /// <summary>
        /// Subtotal before discount.
        /// </summary>
        public decimal SubTotal => Quantity * UnitPrice;
    }
}
