using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Pagination;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Contracts.Persistence
{
    public interface IAuthRepository
    {
        // ── Existing ─────────────────────────────────────────────────────────────
        Task<ApplicationUser?> FindByEmailAsync(string email);
        Task<ApplicationUser?> FindByIdAsync(string userId);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task UpdateAsync(ApplicationUser user);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> CreateRoleAsync(ApplicationRole role);
        Task AddToRoleAsync(ApplicationUser user, string roleName);
        Task AddUserRoleAsync(ApplicationUserRole userRole);
        Task RemoveUserRolesAsync(string userId, string tenantId);
        Task RemoveUserPermissionsAsync(string userId, string tenantId);

        // ── User listing by UserType ──────────────────────────────────────────────
        Task<PagedResult<AccountDto>> GetUsersPagedAsync(
            string tenantId,
            UserType userType,
            string? search,
            int pageNumber,
            int pageSize);

        // ── Role CRUD (tenant-scoped) ─────────────────────────────────────────────
        Task<ApplicationRole?> GetRoleByIdAsync(string roleId, string tenantId);
        Task<PagedResult<RoleDto>> GetRolesPagedAsync(string tenantId, string? search, int pageNumber, int pageSize);
        Task<IdentityResult> UpdateRoleAsync(ApplicationRole role);
        Task<IdentityResult> DeleteRoleAsync(ApplicationRole role);

        // ── User ↔ Role (single role, tenant-scoped) ─────────────────────────────
        Task<ApplicationUserRole?> GetUserRoleAsync(string userId, string roleId, string tenantId);
        Task<IdentityResult> AssignRoleToUserAsync(ApplicationUser user, ApplicationRole role, string tenantId, string assignedBy);
        Task<IdentityResult> RemoveRoleFromUserAsync(ApplicationUser user, ApplicationRole role, string tenantId);
    }
}

