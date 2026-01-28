using Website.Domain.Enums;

namespace Website.Application.Features.OrderFeatures.Queries.GetOrderById
{
    public class GetOrderByIdQueryResponse
    {
        public OrderDetailDto? Order { get; set; }
    }

    public class OrderDetailDto
    {
        public Guid Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public OrderStatus Status { get; set; }

        /// <summary>
        /// Sum of all items before discounts.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Total discount applied to the order.
        /// </summary>
        public decimal DiscountTotal { get; set; }

        /// <summary>
        /// Final amount after discounts.
        /// </summary>
        public decimal TotalAmount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime OrderDate { get; set; }

        public List<OrderItemDto> Items { get; set; } = new();
    }
    public class OrderItemDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Unit price before discount.
        /// </summary>
        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// Subtotal before discount.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Discount applied to this item.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Final price after discount.
        /// </summary>
        public decimal FinalPrice { get; set; }

        /// <summary>
        /// Offer name applied at checkout (snapshot).
        /// </summary>
        public string? AppliedOfferName { get; set; }
    }
}
