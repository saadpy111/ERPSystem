using Identity.Application.Contracts.Persistence;
using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Identity.Persistense.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Persistense.Repositories
{
    public class AuthRepository : IAuthRepository
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IdentityDbContext _context;
        
        public AuthRepository(UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager ,
             RoleManager<ApplicationRole> roleManager,
             IdentityDbContext context
                )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            // Bypass query filters - called during login when no tenant context exists
            var user = await _userManager.Users.IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            // Bypass query filters - user may not have tenant context yet
            return await _userManager.Users.IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> CreateRoleAsync(ApplicationRole role)
        {
            return await _roleManager.CreateAsync(role);
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task AddUserRoleAsync(ApplicationUserRole userRole)
        {
            await _context.Set<ApplicationUserRole>().AddAsync(userRole);
        }

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
    }
}

