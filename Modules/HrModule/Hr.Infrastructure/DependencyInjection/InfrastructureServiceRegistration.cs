using Hr.Application.Contracts.Infrastructure.FileService;
using Hr.Infrastructure.FileService;
using Microsoft.Extensions.DependencyInjection;

namespace Hr.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
     
            return services;
        }
    }
}
