using MediatR;

namespace Identity.Application.Features.AccountFeature.Commands.RemoveRole
{
    public class RemoveRoleCommandRequest : IRequest<RemoveRoleCommandResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}