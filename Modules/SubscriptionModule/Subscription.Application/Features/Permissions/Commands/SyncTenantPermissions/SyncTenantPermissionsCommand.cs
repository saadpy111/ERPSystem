using MediatR;

namespace Subscription.Application.Features.Permissions.Commands.SyncTenantPermissions
{
    public class SyncTenantPermissionsCommand : IRequest<SyncTenantPermissionsResponse>
    {
        public string TenantId { get; set; } = string.Empty;
    }

    public class SyncTenantPermissionsResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
