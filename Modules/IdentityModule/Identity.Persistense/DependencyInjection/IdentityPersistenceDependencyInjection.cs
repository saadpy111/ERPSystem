using Identity.Application.Settings;
using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddDbContext<IdentityDbContext>(options =>
            {
                var con = configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(con);
            });
            #endregion

            return services;
        }
    }
}
