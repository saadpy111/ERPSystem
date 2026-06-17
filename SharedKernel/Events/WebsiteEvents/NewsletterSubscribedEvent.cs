using MediatR;

namespace Events.WebsiteEvents
{
    public class NewsletterSubscribedEvent : INotification
    {
        public string Email { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
    }
}
