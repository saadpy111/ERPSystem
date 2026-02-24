using Website.Domain.Enums;

namespace Website.Application.DTOs
{
    public class AdminOrderListDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? CouponCode { get; set; }
    }

    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal CouponDiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? CouponCode { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public class UserOrderListDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public int ItemCount { get; set; }
    }

    public class UserOrderDetailsDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
    public class AdminDashboardStatsDto
    {
        public decimal TotalRevenue { get; set; }
        public int ProcessingOrdersCount { get; set; }
        public int CompletedOrdersCount { get; set; }
        public int CancelledOrdersCount { get; set; }
    }
}
