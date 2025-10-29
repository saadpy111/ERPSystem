using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Procurement.Application.DependencyInjection;
using Procurement.Infrastructure.DependencyInjection;
using Procurement.Persistence.DependencyInjection;

namespace Procurement.Api.DependencyInjection
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddProcurementApiDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Application services
            services.AddApplicationServices();
            
            // Register Persistence services
            services.AddPersistenceServices(configuration);
            
            // Register Infrastructure services
            services.AddInfrastructureServices();
            
            return services;
        }
    }
}