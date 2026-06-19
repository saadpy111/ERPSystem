using Identity.Application.Features.AccountManagement.Commands.AddPermissionToRole;
using Identity.Application.Features.AccountManagement.Commands.AssignRoleToUser;
using Identity.Application.Features.AccountManagement.Commands.CreateRole;
using Identity.Application.Features.AccountManagement.Commands.CreateUser;
using Identity.Application.Features.AccountManagement.Commands.DeleteRole;
using Identity.Application.Features.AccountManagement.Commands.RemovePermissionFromRole;
using Identity.Application.Features.AccountManagement.Commands.RemoveRoleFromUser;
using Identity.Application.Features.AccountManagement.Commands.ReplaceRolePermissions;
using Identity.Application.Features.AccountManagement.Commands.UpdateRole;
using Identity.Application.Features.AccountManagement.Queries.GetPermissions;
using Identity.Application.Features.AccountManagement.Queries.GetRoles;
using Identity.Application.Features.AccountManagement.Queries.GetUsers;
using Identity.Domain.Enums;
using Identity.Domain.Entities;
using MediatR;
using SharedKernel.Constants.Permissions;
using SharedKernel.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Multitenancy;
using System.Security.Claims;

namespace Identity.Api.Controllers
{
    /// <summary>
    /// Shared base controller with all account-management logic.
    /// Concrete subclasses (ErpAccountsController / WebsiteAccountsController)
    /// inject the correct UserType and route prefix — no duplication.
    /// </summary>
    [ApiController]
    [Authorize]
    [ApiExplorerSettings(GroupName = "Identity")]
    public abstract class AccountManagementBaseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        /// <summary>Concrete subclass must declare which user segment this instance manages.</summary>
        protected abstract UserType ManagedUserType { get; }

        protected AccountManagementBaseController(IMediator mediator , ITenantProvider  tenantProvider)
        {
            _mediator = mediator;
             _tenantProvider = tenantProvider;
        }


        // ── Helpers ───────────────────────────────────────────────────────────────

        private string? TenantId =>
           _tenantProvider.GetTenantId();

        private  RoleScope TargetedRoleScope =>
            ManagedUserType == UserType.Client
                ? RoleScope.Website
                : RoleScope.ERP;

        private string CurrentUserId =>
            User.FindFirstValue("sub") ??
            User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            string.Empty;
        
        private IActionResult MissingTenant() =>
            Unauthorized(new { error = "Tenant context is missing from the token." });

        // ── Accounts ──────────────────────────────────────────────────────────────

        /// <summary>Create a new user in this scope (System or Client).</summary>
        [HttpPost("accounts")]
        [HasPermission(AdminPermissions.UsersCreate)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest body)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new CreateUserCommand
            {
                TenantId = TenantId,
                UserType = ManagedUserType,    // Enforced here — client cannot change it
                FullName = body.FullName,
                Email    = body.Email,
                Password = body.Password,
                PhoneNumber = body.PhoneNumber
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return Ok(new { userId = response.UserId });
        }

        /// <summary>Get all users in this scope (paginated, searchable).</summary>
        [HttpGet("accounts")]
        [HasPermission(AdminPermissions.UsersView)]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new GetUsersQuery
            {
                TenantId   = TenantId,
                UserType   = ManagedUserType,
                Search     = search,
                PageNumber = pageNumber,
                PageSize   = pageSize
            });

            return Ok(response.Result);
        }

        // ── Permissions catalog ───────────────────────────────────────────────────

        /// <summary>
        /// Returns all assignable permissions for this scope.
        /// ERP → system/ERP module permissions only.
        /// Website → website module permissions only.
        /// Filtering logic lives in the handler — controller stays clean.
        /// </summary>
        [HttpGet("permissions")]
        [HasPermission(AdminPermissions.PermissionsView)]
        public async Task<IActionResult> GetPermissions()
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new GetPermissionsQuery
            {
                UserType = ManagedUserType
            });

            return Ok(response.Permissions);
        }
        
        // ── Roles ─────────────────────────────────────────────────────────────────
        
        /// <summary>Get all tenant roles (paginated).</summary>
        [HttpGet("roles")]
        [HasPermission(AdminPermissions.RolesView)]
        public async Task<IActionResult> GetRoles(
            [FromQuery] string? search,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new GetRolesQuery
            {
                TenantId   = TenantId,
                Scope      = TargetedRoleScope,
                Search     = search,
                PageNumber = pageNumber,
                PageSize   = pageSize
            });

            return Ok(response.Result);
        }

        /// <summary>Create a new role in this tenant.</summary>
        [HttpPost("roles")]
        [HasPermission(AdminPermissions.RolesCreate)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest body)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new CreateRoleCommand
            {
                TenantId      = TenantId,
                Name          = body.Name,
                Scope         = TargetedRoleScope,
                PermissionIds = body.PermissionIds
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return Ok(new { roleId = response.RoleId });
        }

        /// <summary>Update (rename) a role.</summary>
        [HttpPut("roles/{roleId}")]
        [HasPermission(AdminPermissions.RolesEdit)]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] UpdateRoleRequest body)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new UpdateRoleCommand
            {
                RoleId   = roleId,
                TenantId = TenantId,
                Name     = body.Name,
                Scope    = TargetedRoleScope
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return NoContent();
        }

        /// <summary>Delete a role.</summary>
        [HttpDelete("roles/{roleId}")]
        [HasPermission(AdminPermissions.RolesDelete)]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new DeleteRoleCommand
            {
                RoleId   = roleId,
                TenantId = TenantId,
                Scope    = TargetedRoleScope
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return NoContent();
        }

        // ── Role Permissions ──────────────────────────────────────────────────────

        /// <summary>Add a single permission to a role.</summary>
        [HttpPost("roles/{roleId}/permissions")]
        [HasPermission(AdminPermissions.RolesAssignPermissions)]
        public async Task<IActionResult> AddPermissionToRole(
            string roleId, [FromBody] SinglePermissionRequest body)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new AddPermissionToRoleCommand
            {
                RoleId       = roleId,
                TenantId     = TenantId,
                PermissionId = body.PermissionId,
                Scope        = TargetedRoleScope
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return NoContent();
        }

        /// <summary>Remove a single permission from a role.</summary>
        [HttpDelete("roles/{roleId}/permissions/{permissionId}")]
        [HasPermission(AdminPermissions.RolesRemovePermissions)]
        public async Task<IActionResult> RemovePermissionFromRole(string roleId, string permissionId)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new RemovePermissionFromRoleCommand
            {
                RoleId       = roleId,
                TenantId     = TenantId,
                PermissionId = permissionId,
                Scope        = TargetedRoleScope
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return NoContent();
        }

        /// <summary>Replace ALL permissions of a role atomically.</summary>
        [HttpPut("roles/{roleId}/permissions")]
        [HasPermission(AdminPermissions.RolesAssignPermissions)]
        public async Task<IActionResult> ReplaceRolePermissions(
            string roleId, [FromBody] ReplacePermissionsRequest body)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new ReplaceRolePermissionsCommand
            {
                RoleId        = roleId,
                TenantId      = TenantId,
                PermissionIds = body.PermissionIds,
                Scope         = TargetedRoleScope
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return NoContent();
        }

        // ── User ↔ Role ───────────────────────────────────────────────────────────

        /// <summary>Assign a role to a user.</summary>
        [HttpPost("users/{userId}/roles/{roleId}")]
        [HasPermission(AdminPermissions.UsersAssignRoles)]
        public async Task<IActionResult> AssignRoleToUser(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new AssignRoleToUserCommand
            {
                UserId     = userId,
                RoleId     = roleId,
                TenantId   = TenantId,
                AssignedBy = CurrentUserId
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return NoContent();
        }

        /// <summary>Remove a role from a user.</summary>
        [HttpDelete("users/{userId}/roles/{roleId}")]
        [HasPermission(AdminPermissions.UsersRemoveRoles)]
        public async Task<IActionResult> RemoveRoleFromUser(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(TenantId)) return MissingTenant();

            var response = await _mediator.Send(new RemoveRoleFromUserCommand
            {
                UserId   = userId,
                RoleId   = roleId,
                TenantId = TenantId
            });

            if (!response.Success) return BadRequest(new { error = response.Error });
            return NoContent();
        }
    }

    // ── Request body contracts ────────────────────────────────────────────────────

    public record CreateUserRequest(string FullName, string Email, string Password, string PhoneNumber);
    public record CreateRoleRequest(string Name, List<string> PermissionIds);
    public record UpdateRoleRequest(string Name);
    public record SinglePermissionRequest(string PermissionId);
    public record ReplacePermissionsRequest(List<string> PermissionIds);
}
