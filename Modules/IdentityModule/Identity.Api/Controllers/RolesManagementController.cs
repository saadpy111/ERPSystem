using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Authorization;
using SharedKernel.Constants;
using SharedKernel.Constants.Permissions;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Identity")]
    public class RolesManagementController : ControllerBase
    {
        private readonly IdentityDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RolesManagementController(IdentityDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        [HasPermission(AdminPermissions.RolesView)]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet("{id}/permissions")]
        [HasPermission(AdminPermissions.PermissionsView)]
        public async Task<IActionResult> GetRolePermissions(string id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return NotFound("Role not found");

            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == id)
                .Include(rp => rp.Permission)
                .Select(rp => new
                {
                    rp.Permission.Id,
                    rp.Permission.Name,
                    rp.Permission.Module,
                    rp.Permission.Description,
                    rp.AssignedAt
                })
                .ToListAsync();

            return Ok(new
            {
                Role = new { role.Id, role.Name },
                Permissions = permissions
            });
        }

        [HttpPost("{roleId}/users/{userId}")]
        [HasPermission(AdminPermissions.RolesAssignPermissions)]

        public async Task<IActionResult> AssignRoleToUser(string roleId, string userId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
                return NotFound("Role not found");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found");

            // Check if already assigned
            var existing = await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
            if (existing)
                return BadRequest("Role already assigned to user");

            var userRole = new ApplicationUserRole
            {
                UserId = userId,
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow,
                AssignedBy = User.Identity?.Name ?? "System",
               
            };

            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Role assigned to user successfully" });
        }

        [HttpDelete("{roleId}/users/{userId}")]
        [HasPermission(AdminPermissions.UsersRemoveRoles)]

        public async Task<IActionResult> RemoveRoleFromUser(string roleId, string userId)
        {
            var userRole = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (userRole == null)
                return NotFound("Role assignment not found");

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Role removed from user successfully" });
        }
    }
}
