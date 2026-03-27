using Identity.Domain.Entities;
using Identity.Domain.Enums;
using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.UpdateRole
{
    public class UpdateRoleCommand : IRequest<UpdateRoleResponse>
    {
        public string RoleId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public RoleScope Scope { get; set; }
    }

    public class UpdateRoleResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
