using System;

namespace Website.Application.DTOs
{
    public class NewsletterSubscriberDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime SubscribedAt { get; set; }
    }
}
