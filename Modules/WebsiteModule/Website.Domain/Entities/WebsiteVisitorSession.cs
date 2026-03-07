using System;

namespace Website.Domain.Entities
{
    public class WebsiteVisitorSession : BaseEntity
    {
        public string SessionId { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime LastSeenAt { get; set; }
    }
}
