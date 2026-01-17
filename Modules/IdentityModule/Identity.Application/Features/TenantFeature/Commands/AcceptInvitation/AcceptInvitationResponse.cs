namespace Identity.Application.Features.TenantFeature.Commands.AcceptInvitation
{
    public class AcceptInvitationResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? TenantId { get; set; }
        public string? TenantName { get; set; }
        public string? NewToken { get; set; } // JWT with tenant context
    }
}
