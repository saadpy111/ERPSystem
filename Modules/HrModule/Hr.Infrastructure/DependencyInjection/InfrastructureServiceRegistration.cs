using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Infrastructure.FileService;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register infrastructure services here when needed
            services.AddScoped<IFileService, HrLocalFileService>(provider =>
                new HrLocalFileService("wwwroot/uploads"));
            
            return services;
        }
    }
}
