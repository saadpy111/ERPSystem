using MediatR;

namespace Events.WebsiteEvents
{
    public class AddToCartEvent : INotification
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public string? SessionId { get; set; }
    }
}
