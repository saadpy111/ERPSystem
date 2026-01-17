using Identity.Application.Contracts.Persistence;
using Identity.Application.Settings;
using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Identity.Persistense.Repositories;
using Identity.Persistense.Repositories.GenericRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Multitenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Persistense.DependencyInjection
{
    public static class IdentityPersistenceDependencyInjection
    {
        public static IServiceCollection AddIdentityPersistenceDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            services.AddDbContext<IdentityDbContext>((sp, options) =>
            {
                var tenantProvider = sp.GetRequiredService<ITenantProvider>();

                var con = configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(con);

            }, ServiceLifetime.Scoped);
            #endregion




            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<ITenantInvitationRepository, TenantInvitationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
