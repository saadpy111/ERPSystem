using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.DependencyInjection
{
    public static class IdentityApplicationDependencyInjection
    {
        public static IServiceCollection AddIdentityApplicationDependencyInjection(this IServiceCollection services , IConfiguration configuration)
        {
            return services;
        }
    }
}
