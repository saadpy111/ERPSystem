namespace SharedKernel.Subscription
{
    public class RoleProvisioningResult
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public interface ITenantRoleProvisioningService
    {
        Task<RoleProvisioningResult> ProvisionRolesAsync(
            string tenantId,
            List<string> effectiveModuleCodes);
    }
}
