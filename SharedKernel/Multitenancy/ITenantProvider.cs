namespace SharedKernel.Multitenancy
{
    public interface ITenantProvider
    {
        string? GetTenantId();
        void SetTenantId(string tenantId);
    }
}
