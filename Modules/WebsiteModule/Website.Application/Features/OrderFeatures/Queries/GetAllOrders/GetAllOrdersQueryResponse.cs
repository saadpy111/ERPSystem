using Website.Domain.Enums;

namespace Website.Application.Features.OrderFeatures.Queries.GetAllOrders
{
    public class GetAllOrdersQueryResponse
    {
        public List<AdminOrderDto> Orders { get; set; } = new();
    }

    public class AdminOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
