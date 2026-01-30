using Microsoft.EntityFrameworkCore;
using Website.Application.Contracts.Persistence;
using Website.Domain.Entities;
using Website.Persistence.Context;

namespace Website.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for TenantWebsite entity.
    /// </summary>
    public class TenantWebsiteRepository : ITenantWebsiteRepository
    {
        private readonly WebsiteDbContext _context;

        public TenantWebsiteRepository(WebsiteDbContext context)
        {
            _context = context;
        }

        public async Task<TenantWebsite?> GetByIdAsync(Guid id)
        {
            return await _context.TenantWebsites.FirstOrDefaultAsync(tw => tw.Id == id);
        }

        public async Task<TenantWebsite?> GetByTenantIdAsync(string tenantId)
        {
            return await _context.TenantWebsites
                .FirstOrDefaultAsync(tw => tw.TenantId == tenantId);
        }

        public async Task<TenantWebsite?> GetByDomainAsync(string domain)
        {
            // Search by domain in SiteConfig (JSON column)
            return await _context.TenantWebsites.IgnoreQueryFilters()
                .FirstOrDefaultAsync
                (tw =>( tw.Config.Domain.ToLower() == domain.ToLower() && tw.IsPublished ));
        }

        public async Task<TenantWebsite> CreateAsync(TenantWebsite tenantWebsite)
        {
            tenantWebsite.CreatedAt = DateTime.UtcNow;
            tenantWebsite.UpdatedAt = DateTime.UtcNow;
            
            await _context.TenantWebsites.AddAsync(tenantWebsite);
            return tenantWebsite;
        }

        public async Task<TenantWebsite> UpdateAsync(TenantWebsite tenantWebsite)
        {
            tenantWebsite.UpdatedAt = DateTime.UtcNow;
            _context.TenantWebsites.Update(tenantWebsite);
            return await Task.FromResult(tenantWebsite);
        }

        public async Task<bool> ExistsAsync(string tenantId)
        {
            return await _context.TenantWebsites.AnyAsync(tw => tw.TenantId == tenantId);
        }
    }
}
