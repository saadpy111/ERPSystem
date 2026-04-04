using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Api.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountingApiServices(this IServiceCollection services)
        {
            // Register controllers
            services.AddControllers();

            // Register API-specific services here
            
            // Register Authorization policies if needed (placeholders)
            // services.AddAuthorization(options => ...);

            return services;
        }
    }
}
