using Identity.Application.Contracts.Persistence;
using Identity.Application.Dtos.AccountDtos;
using Identity.Application.Pagination;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Persistense.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Domain.Extensions;

namespace Identity.Persistense.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IdentityDbContext _context;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IdentityDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        // ── Existing ──────────────────────────────────────────────────────────────

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
            => await _userManager.Users.IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<ApplicationUser?> FindByIdAsync(string userId)
            => await _userManager.Users.IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == userId);

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
            => await _userManager.GetRolesAsync(user);

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task UpdateAsync(ApplicationUser user)
            => await _userManager.UpdateAsync(user);

        public async Task<bool> RoleExistsAsync(string roleName)
            => await _roleManager.RoleExistsAsync(roleName);

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
            => await _roleManager.CreateAsync(role);

        public async Task AddToRoleAsync(ApplicationUser user, string roleName)
            => await _userManager.AddToRoleAsync(user, roleName);

        public async Task AddUserRoleAsync(ApplicationUserRole userRole)
            => await _context.Set<ApplicationUserRole>().AddAsync(userRole);

        public async Task RemoveUserRolesAsync(string userId, string tenantId)
        {
            var userRoles = await _context.Set<ApplicationUserRole>()
                .Where(ur => ur.UserId == userId && ur.TenantId == tenantId)
                .ToListAsync();
            _context.Set<ApplicationUserRole>().RemoveRange(userRoles);
        }

        public async Task RemoveUserPermissionsAsync(string userId, string tenantId)
        {
            var userPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId && up.TenantId == tenantId)
                .ToListAsync();
            _context.UserPermissions.RemoveRange(userPermissions);
        }

        // ── User listing by UserType ──────────────────────────────────────────────

        public async Task<PagedResult<AccountDto>> GetUsersPagedAsync(
            string tenantId, UserType userType, string? search, int pageNumber, int pageSize)
        {
            var query = _userManager.Users
                .IgnoreQueryFilters()
                .Where(u => u.TenantId == tenantId && u.UserType == userType);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(u =>
                    (u.FullName != null && u.FullName.ToLower().Contains(term)) ||
                    (u.Email    != null && u.Email.ToLower().Contains(term)));
            }

            var total = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();

            var dtos = users.Select(u => new AccountDto
            {
                Id       = u.Id,
                UserName = u.UserName ?? string.Empty,
                Email    = u.Email    ?? string.Empty,
                FullName = u.FullName ?? string.Empty,
                Roles    = u.UserRoles
                            .Where(ur => ur.TenantId == tenantId)
                            .Select(ur => (ur.Role?.Name ?? string.Empty).ToCleanRoleName(tenantId))
                            .ToList()
            }).ToList();

            return new PagedResult<AccountDto> { Items = dtos, TotalCount = total };
        }

        // ── Role CRUD (tenant-scoped) ─────────────────────────────────────────────

        public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId, string tenantId)
            => await _roleManager.Roles
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == roleId && r.TenantId == tenantId);

        public async Task<PagedResult<RoleDto>> GetRolesPagedAsync(
            string tenantId ,RoleScope scope, string? search, int pageNumber, int pageSize)
        {
            var query = _roleManager.Roles
                .IgnoreQueryFilters()
                .Where(r => r.TenantId == tenantId && r.Scope == scope);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(r => r.Name != null && r.Name.ToLower().Contains(term));
            }

            var total = await query.CountAsync();

            var roles = await query
                .OrderBy(r => r.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();

            var dtos = roles.Select(r => new RoleDto
            {
                Id          = r.Id,
                Name        = (r.Name ?? string.Empty).ToCleanRoleName(tenantId),
                TenantId    = r.TenantId,
                Permissions = r.RolePermissions
                               .Where(rp => rp.TenantId == tenantId)
                               .Select(rp => rp.Permission?.Name ?? string.Empty)
                               .ToList()
            }).ToList();

            return new PagedResult<RoleDto> { Items = dtos, TotalCount = total };
        }

        public async Task<IdentityResult> UpdateRoleAsync(ApplicationRole role)
        {
            role.NormalizedName = role.Name?.ToUpperInvariant();
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(ApplicationRole role)
            => await _roleManager.DeleteAsync(role);

        // ── User ↔ Role (single role, tenant-scoped) ─────────────────────────────

        public async Task<ApplicationUserRole?> GetUserRoleAsync(
            string userId, string roleId, string tenantId)
            => await _context.Set<ApplicationUserRole>()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(ur =>
                    ur.UserId == userId &&
                    ur.RoleId == roleId &&
                    ur.TenantId == tenantId);

        public async Task<IdentityResult> AssignRoleToUserAsync(
            ApplicationUser user, ApplicationRole role, string tenantId, string assignedBy)
        {
            // Verify the role belongs to this tenant before assigning
            if (role.TenantId != tenantId)
                return IdentityResult.Failed(
                    new IdentityError { Code = "CrossTenant", Description = "Role does not belong to this tenant." });

            var existing = await GetUserRoleAsync(user.Id, role.Id, tenantId);
            if (existing != null)
                return IdentityResult.Failed(
                    new IdentityError { Code = "AlreadyAssigned", Description = "User already has this role." });

            var userRole = new ApplicationUserRole
            {
                UserId     = user.Id,
                RoleId     = role.Id,
                TenantId   = tenantId,
                AssignedAt = DateTime.UtcNow,
                AssignedBy = assignedBy
            };
            await _context.Set<ApplicationUserRole>().AddAsync(userRole);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> RemoveRoleFromUserAsync(
            ApplicationUser user, ApplicationRole role, string tenantId)
        {
            var userRole = await GetUserRoleAsync(user.Id, role.Id, tenantId);
            if (userRole == null)
                return IdentityResult.Failed(
                    new IdentityError { Code = "NotFound", Description = "User does not have this role." });

            _context.Set<ApplicationUserRole>().Remove(userRole);
            return IdentityResult.Success;
        }
    }
}
