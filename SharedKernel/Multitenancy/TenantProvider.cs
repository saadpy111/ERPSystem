namespace SharedKernel.Multitenancy
{
    public class TenantProvider : ITenantProvider
    {
        private static readonly AsyncLocal<string?> _tenantId = new AsyncLocal<string?>();

        public string? GetTenantId()
        {
            return _tenantId.Value;
        }

        public void SetTenantId(string tenantId)
        {
            _tenantId.Value = tenantId;
        }
    }
}
