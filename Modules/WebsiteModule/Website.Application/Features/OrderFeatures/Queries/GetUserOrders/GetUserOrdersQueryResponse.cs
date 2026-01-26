using Website.Domain.Enums;

namespace Website.Application.Features.OrderFeatures.Queries.GetUserOrders
{
    public class GetUserOrdersQueryResponse
    {
        public List<UserOrderDto> Orders { get; set; } = new();
    }

    public class UserOrderDto
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
