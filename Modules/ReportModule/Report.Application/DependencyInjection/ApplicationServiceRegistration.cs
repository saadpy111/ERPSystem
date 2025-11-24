using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Application.Contracts.Persistence.Repositories;

namespace Report.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddReportApplicationServices(this IServiceCollection services , IConfiguration configuration)
        {
          

            return services;
        }
    }
}