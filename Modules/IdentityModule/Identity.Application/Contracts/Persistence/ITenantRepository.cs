using Identity.Domain.Entities;

namespace Identity.Application.Contracts.Persistence
{
    public interface ITenantRepository
    {
        Task<Tenant?> GetByCodeAsync(string code);
        Task<Tenant?> GetByIdAsync(string id);
        Task<Tenant> CreateAsync(Tenant tenant);
        Task<bool> ExistsAsync(string code);
    }
}
