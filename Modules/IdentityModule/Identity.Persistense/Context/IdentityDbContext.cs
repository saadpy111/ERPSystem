using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Multitenancy;

namespace Identity.Persistense.Context
{
    public class IdentityDbContext : IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        string,
        IdentityUserClaim<string>,
        ApplicationUserRole,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>
    {
    
        public string? CurrentTenantId { get; }

        public IdentityDbContext(
            DbContextOptions<IdentityDbContext> options,
            ITenantProvider? tenantProvider = null)
            : base(options)
        {
            CurrentTenantId = tenantProvider?.GetTenantId();
        }


        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<UserPermission> UserPermissions => Set<UserPermission>();
        public DbSet<TenantInvitation> TenantInvitations => Set<TenantInvitation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Identity");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

            ApplyGlobalQueryFilters(modelBuilder);
        }

        private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            // Only apply tenant filters when CurrentTenantId exists
            // Pre-tenant users (TenantId = NULL) bypass filters for identity operations
            if (!string.IsNullOrEmpty(CurrentTenantId))
            {
                // Filter tenant-scoped entities
                modelBuilder.Entity<ApplicationRole>()
                    .HasQueryFilter(r => r.TenantId == CurrentTenantId);

                modelBuilder.Entity<RolePermission>()
                    .HasQueryFilter(rp => rp.TenantId == CurrentTenantId);

                modelBuilder.Entity<UserPermission>()
                    .HasQueryFilter(up => up.TenantId == CurrentTenantId);

                modelBuilder.Entity<TenantInvitation>()
                    .HasQueryFilter(ti => ti.TenantId == CurrentTenantId);
                    
                // Note: ApplicationUser filter removed - handled explicitly in repositories
                // Note: Permission is global and never filtered
            }
        }
    }
}
