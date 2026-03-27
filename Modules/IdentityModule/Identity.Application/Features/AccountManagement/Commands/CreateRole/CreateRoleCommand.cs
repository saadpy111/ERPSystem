using Identity.Application.Features.AccountManagement.Commands.CreateRole;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.CreateRole
{
    public class CreateRoleCommand : IRequest<CreateRoleResponse>
    {
        public string TenantId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public RoleScope Scope { get; set; }
        public List<string> PermissionIds { get; set; } = new();
    }

    public class CreateRoleResponse
    {
        public bool Success { get; set; }
        public string? RoleId { get; set; }
        public string? Error { get; set; }
    }
}
