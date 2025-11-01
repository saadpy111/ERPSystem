using Hr.Application.DependencyInjection;
using Hr.Infrastructure.DependencyInjection;
using Hr.Persistence.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.Api.DependencyInjection
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddHrApiDependencyInjection(this IServiceCollection services, IConfiguration configuration)
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
