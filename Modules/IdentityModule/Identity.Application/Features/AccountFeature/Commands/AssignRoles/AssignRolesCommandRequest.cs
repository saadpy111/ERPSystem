using MediatR;

namespace Identity.Application.Features.AccountFeature.Commands.AssignRoles
{
    public class AssignRolesCommandRequest : IRequest<AssignRolesCommandResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}