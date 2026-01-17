using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Persistense.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistense.Repositories
{
    public class TenantInvitationRepository : ITenantInvitationRepository
    {
        private readonly IdentityDbContext _context;

        public TenantInvitationRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<TenantInvitation?> GetByTokenAsync(string token)
        {
            return await _context.TenantInvitations
                .Include(ti => ti.Tenant)
                .Include(ti => ti.Role)
                .FirstOrDefaultAsync(ti => ti.InvitationToken == token);
        }

        public async Task<TenantInvitation?> GetByIdAsync(string id)
        {
            return await _context.TenantInvitations
                .Include(ti => ti.Tenant)
                .Include(ti => ti.Role)
                .FirstOrDefaultAsync(ti => ti.Id == id);
        }

        public async Task<List<TenantInvitation>> GetByTenantIdAsync(string tenantId)
        {
            return await _context.TenantInvitations
                .Where(ti => ti.TenantId == tenantId)
                .Include(ti => ti.Role)
                .OrderByDescending(ti => ti.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TenantInvitation>> GetByEmailAsync(string email)
        {
            return await _context.TenantInvitations
                .IgnoreQueryFilters() // User may not have tenant yet
                .Where(ti => ti.Email == email.ToLower() && ti.Status == InvitationStatus.Pending)
                .Include(ti => ti.Tenant)
                .Include(ti => ti.Role)
                .ToListAsync();
        }

        public async Task CreateAsync(TenantInvitation invitation)
        {
            await _context.TenantInvitations.AddAsync(invitation);
        }

        public async Task UpdateAsync(TenantInvitation invitation)
        {
            _context.TenantInvitations.Update(invitation);
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            return await _context.TenantInvitations
                .IgnoreQueryFilters() // Check globally
                .AnyAsync(ti => 
                    ti.InvitationToken == token && 
                    ti.Status == InvitationStatus.Pending &&
                    ti.ExpiresAt > DateTime.UtcNow);
        }
    }
}
