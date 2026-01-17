using Identity.Domain.Entities;

namespace Identity.Application.Contracts.Persistence
{
    public interface ITenantInvitationRepository
    {
        Task<TenantInvitation?> GetByTokenAsync(string token);
        Task<TenantInvitation?> GetByIdAsync(string id);
        Task<List<TenantInvitation>> GetByTenantIdAsync(string tenantId);
        Task<List<TenantInvitation>> GetByEmailAsync(string email);
        Task CreateAsync(TenantInvitation invitation);
        Task UpdateAsync(TenantInvitation invitation);
        Task<bool> IsTokenValidAsync(string token);
    }
}
