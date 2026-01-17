namespace Identity.Application.Features.TenantFeature.Commands.SendInvitation
{
    public class SendInvitationResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? InvitationToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
