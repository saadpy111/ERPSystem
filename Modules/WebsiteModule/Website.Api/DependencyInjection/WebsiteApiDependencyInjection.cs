using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Website.Application.DependencyInjection;
using Website.Persistence.DependencyInjection;
using Website.Infrastructure.DependencyInjection;

namespace Website.Api.DependencyInjection
{
    public static class WebsiteApiDependencyInjection
    {
        public static IServiceCollection AddWebsiteApiDependencyInjection(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // Register lower layers
            services.AddWebsitePersistenceDependencyInjection(configuration);
            services.AddWebsiteApplicationDependencyInjection(configuration);
            services.AddWebsiteInfrastructureDependencyInjection();

            return services;
        }
    }
}
