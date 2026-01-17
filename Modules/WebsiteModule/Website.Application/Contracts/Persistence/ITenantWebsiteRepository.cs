using System;
using System.Threading.Tasks;
using Website.Domain.Entities;

namespace Website.Application.Contracts.Persistence
{
    /// <summary>
    /// Repository interface for TenantWebsite entity.
    /// </summary>
    public interface ITenantWebsiteRepository
    {
        Task<TenantWebsite?> GetByIdAsync(Guid id);
        Task<TenantWebsite?> GetByTenantIdAsync(string tenantId);
        Task<TenantWebsite?> GetByDomainAsync(string domain);
        Task<TenantWebsite> CreateAsync(TenantWebsite tenantWebsite);
        Task<TenantWebsite> UpdateAsync(TenantWebsite tenantWebsite);
        Task<bool> ExistsAsync(string tenantId);
    }
}
