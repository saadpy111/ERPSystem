using System;

namespace Website.Domain.Entities
{
    public class NewsletterSubscriber : BaseEntity
    {
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UnsubscribedAt { get; set; }
    }
}
