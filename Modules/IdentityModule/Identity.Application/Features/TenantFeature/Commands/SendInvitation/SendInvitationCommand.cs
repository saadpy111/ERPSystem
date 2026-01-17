using MediatR;

namespace Identity.Application.Features.TenantFeature.Commands.SendInvitation
{
    public class SendInvitationCommand : IRequest<SendInvitationResponse>
    {
        public string InvitedByUserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
    }
}
