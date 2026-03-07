using MediatR;

namespace Events.WebsiteEvents
{
    public class CheckoutStartedEvent : INotification
    {
        public Guid CartId { get; set; }
        public string TenantId { get; set; } = string.Empty;
        public string? SessionId { get; set; }
    }
}
