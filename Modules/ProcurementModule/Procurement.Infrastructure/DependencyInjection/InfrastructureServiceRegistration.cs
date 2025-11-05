using Microsoft.Extensions.DependencyInjection;
using Procurement.Application.Contracts.Infrastructure.FileService;
using Procurement.Infrastructure.FileService;

namespace Procurement.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register infrastructure services            
            return services;
        }
    }
}