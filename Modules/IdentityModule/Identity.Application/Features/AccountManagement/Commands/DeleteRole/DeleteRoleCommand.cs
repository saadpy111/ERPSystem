using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequest<DeleteRoleResponse>
    {
        public string RoleId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
    }

    public class DeleteRoleResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
