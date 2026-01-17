using MediatR;

namespace Identity.Application.Features.TenantFeature.Commands.AcceptInvitation
{
    public class AcceptInvitationCommand : IRequest<AcceptInvitationResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string InvitationToken { get; set; } = string.Empty;
    }
}
