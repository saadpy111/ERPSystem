using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Persistence
{
    public interface IAuthRepository
    {
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
    }
}
