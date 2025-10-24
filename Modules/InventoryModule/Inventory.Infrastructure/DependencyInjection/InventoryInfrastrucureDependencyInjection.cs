using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Inventory.Infrastructure.DependencyInjection
{
    public static class InventoryInfrastrucureDependencyInjection
    {
        public static IServiceCollection AddInventoryInfrastrucureDependencyInjection(this IServiceCollection services , IConfiguration configuration )
        {
            return services;
        }
    }
}
