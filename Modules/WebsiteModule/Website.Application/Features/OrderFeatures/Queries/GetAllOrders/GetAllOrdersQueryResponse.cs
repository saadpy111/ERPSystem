using Website.Domain.Enums;

namespace Website.Application.Features.OrderFeatures.Queries.GetAllOrders
{
    public class GetAllOrdersQueryResponse
    {
        public List<AdminOrderDto> Orders { get; set; } = new();
    }

    /// <summary>
    /// Lightweight DTO for admin orders list view.
    /// </summary>
    public class AdminOrderDto
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

        public int ItemCount { get; set; }

        public DateTime OrderDate { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}
