using Identity.Domain.Entities;

namespace Identity.Application.Services
{
    public class RoleProvisioningResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public List<ApplicationRole> CreatedRoles { get; set; } = new();
    }

    public interface ITenantRoleProvisioningService
    {
        Task<RoleProvisioningResult> ProvisionRolesAsync(
            string tenantId,
            List<string> effectiveModuleCodes);
    }
}
