using MediatR;

namespace Identity.Application.Features.AccountManagement.Commands.AddPermissionToRole
{
    public class AddPermissionToRoleCommand : IRequest<RolePermissionResponse>
    {
        public string RoleId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        /// <summary>Permission name (e.g. "Inventory.Products.View") or permission ID.</summary>
        public string PermissionId { get; set; } = string.Empty;
    }

    public class RolePermissionResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
