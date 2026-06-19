using MediatR;

namespace Events.IdentityEvents
{
    public class ClientRegisteredEvent : INotification
    {
        public string UserId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
