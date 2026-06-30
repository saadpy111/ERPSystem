namespace SharedKernel.Contracts
{
    public record TenantInfo(bool Exists, bool IsActive);

    public interface ITenantReadService
    {
        Task<TenantInfo?> GetTenantInfoAsync(string tenantId);
    }
}
