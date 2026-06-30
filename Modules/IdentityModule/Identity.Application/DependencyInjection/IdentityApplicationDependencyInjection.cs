using Identity.Application.Contracts.Persistence;
using Identity.Application.Services;
using SharedKernel.Authorization;
using SharedKernel.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Identity.Application.DependencyInjection
{
    public static class IdentityApplicationDependencyInjection
    {
        public static IServiceCollection AddIdentityApplicationDependencyInjection(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<IUserLookupService, UserLookupService>();

            // Hybrid permission service: token-first then DB fallback
            services.AddScoped<IPermissionService, PermissionService>();

            // Permission synchronization service
            services.AddScoped<IPermissionSynchronizationService, PermissionSynchronizationService>();

            // Module-to-role mapping (single source of truth)
            services.AddSingleton<IModuleRoleMappingService, ModuleRoleMappingService>();

            // Tenant role provisioning service (dynamic role creation + permission sync)
            services.AddScoped<ITenantRoleProvisioningService, TenantRoleProvisioningService>();

            // Tenant read service (cross-module boundary)
            services.AddScoped<ITenantReadService, TenantReadService>();

            return services;
        }
    }
}
