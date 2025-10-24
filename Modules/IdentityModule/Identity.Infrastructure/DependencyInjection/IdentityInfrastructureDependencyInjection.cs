using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.DependencyInjection
{
    public static class IdentityInfrastructureDependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructureDependencyInjection(this IServiceCollection services , IConfiguration configuration)
        {
            return services;
        }
    }
}
