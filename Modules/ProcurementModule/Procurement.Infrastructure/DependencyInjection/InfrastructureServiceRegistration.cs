using Microsoft.Extensions.DependencyInjection;
using Procurement.Infrastructure.Services;

namespace Procurement.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register infrastructure services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStockUpdateService, StockUpdateService>();
            
            return services;
        }
    }
}