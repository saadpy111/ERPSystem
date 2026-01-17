using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistense.Seeders
{
    public class RoleSeeder
    {
        private readonly IdentityDbContext _context;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleSeeder(IdentityDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            var tenants = await _context.Tenants.IgnoreQueryFilters().ToListAsync();

            foreach (var tenant in tenants)
            {
                // Check if roles already exist for this tenant (bypass query filters)
                if (await _context.Roles.IgnoreQueryFilters().AnyAsync(r => r.TenantId == tenant.Id))
                {
                    continue; // Already seeded
                }

                // Define roles for each tenant
                var roleNames = new[]
                {
                    "SuperAdmin",
                    "InventoryManager",
                    "HRManager",
                    "ProcurementManager",
                    "ReportViewer"
                };

                foreach (var roleName in roleNames)
                {
                    // Create tenant-specific role name to avoid ASP.NET Identity's unique index on NormalizedName
                    var tenantRoleName = $"{roleName}_{tenant.Code}"; // e.g., "SuperAdmin_DEFAULT"
                    
                    // Check if role already exists using RoleManager
                    var existingRole = await _roleManager.FindByNameAsync(tenantRoleName);
                    if (existingRole != null)
                    {
                        continue; // Role already exists
                    }

                    var role = new ApplicationRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = tenantRoleName,  // Unique name per tenant
                        NormalizedName = tenantRoleName.ToUpper(),
                        TenantId = tenant.Id
                    };

                    var result = await _roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to create role {tenantRoleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }

                await _context.SaveChangesAsync();

                // Assign permissions to roles
                await AssignPermissionsToRoles(tenant.Id, tenant.Code);
            }
        }

        private async Task AssignPermissionsToRoles(string tenantId, string tenantCode)
        {
            // Get roles for this tenant using tenant-specific names (bypass query filters)
            var superAdminRole = await _context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Name == $"SuperAdmin_{tenantCode}" && r.TenantId == tenantId);
            var inventoryManagerRole = await _context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Name == $"InventoryManager_{tenantCode}" && r.TenantId == tenantId);
            var hrManagerRole = await _context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Name == $"HRManager_{tenantCode}" && r.TenantId == tenantId);
            var procurementManagerRole = await _context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Name == $"ProcurementManager_{tenantCode}" && r.TenantId == tenantId);
            var reportViewerRole = await _context.Roles.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Name == $"ReportViewer_{tenantCode}" && r.TenantId == tenantId);

            // Get ALL global permissions (no tenant filter - permissions are global)
            var allPermissions = await _context.Permissions.ToListAsync();
            var inventoryPermissions = allPermissions.Where(p => p.Module == "Inventory").ToList();
            var hrPermissions = allPermissions.Where(p => p.Module == "HR").ToList();
            var procurementPermissions = allPermissions.Where(p => p.Module == "Procurement").ToList();
            var reportViewPermissions = allPermissions.Where(p => p.Module == "Report").ToList();

            var rolePermissions = new List<RolePermission>();

            // SuperAdmin gets all permissions
            if (superAdminRole != null)
            {
                foreach (var permission in allPermissions)
                {
                    rolePermissions.Add(new RolePermission
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = superAdminRole.Id,
                        PermissionId = permission.Id,
                        TenantId = tenantId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            // InventoryManager gets inventory permissions
            if (inventoryManagerRole != null)
            {
                foreach (var permission in inventoryPermissions)
                {
                    rolePermissions.Add(new RolePermission
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = inventoryManagerRole.Id,
                        PermissionId = permission.Id,
                        TenantId = tenantId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            // HRManager gets HR permissions
            if (hrManagerRole != null)
            {
                foreach (var permission in hrPermissions)
                {
                    rolePermissions.Add(new RolePermission
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = hrManagerRole.Id,
                        PermissionId = permission.Id,
                        TenantId = tenantId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            // ProcurementManager gets procurement permissions
            if (procurementManagerRole != null)
            {
                foreach (var permission in procurementPermissions)
                {
                    rolePermissions.Add(new RolePermission
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = procurementManagerRole.Id,
                        PermissionId = permission.Id,
                        TenantId = tenantId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            // ReportViewer gets report view permissions
            if (reportViewerRole != null)
            {
                foreach (var permission in reportViewPermissions)
                {
                    rolePermissions.Add(new RolePermission
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = reportViewerRole.Id,
                        PermissionId = permission.Id,
                        TenantId = tenantId,
                        AssignedAt = DateTime.UtcNow
                    });
                }
            }

            await _context.RolePermissions.AddRangeAsync(rolePermissions);
            await _context.SaveChangesAsync();
        }
    }
}
