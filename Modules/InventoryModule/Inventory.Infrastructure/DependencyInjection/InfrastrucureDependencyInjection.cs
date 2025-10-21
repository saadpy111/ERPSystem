using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Inventory.Infrastructure.DependencyInjection
{
    public static class InfrastrucureDependencyInjection
    {
        public static IServiceCollection AddInfrastrucureDependencyInjection(this IServiceCollection services , IConfiguration configuration )
        {
            return services;
        }
    }
}
