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
        public decimal TotalAmount { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
