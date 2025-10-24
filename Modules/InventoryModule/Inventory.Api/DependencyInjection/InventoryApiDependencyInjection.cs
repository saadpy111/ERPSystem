using Inventory.Application.DependencyInjection;
using Inventory.Infrastructure.DependencyInjection;
using Inventory.Persistence.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.DependencyInjection
{
    public static class InventoryApiDependencyInjection
    {
        public static IServiceCollection AddPreLayersInventoryDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInventoryPersistenceDependencyInjection(configuration);
            services.AddInventoryInfrastrucureDependencyInjection(configuration);
            services.AddInventoryApplicationDependencyInjection(configuration);
            return services;
        }
        public static IServiceCollection AddInventoryApiDependencyInjection(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddPreLayersInventoryDependencyInjection(configuration);
            return services;
        }
    }
}

