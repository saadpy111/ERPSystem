using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Application.DependencyInjection;
using Report.Persistence.DependencyInjection;

namespace Report.Api.DependencyInjection
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddReportApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddReportPersistenceServices(configuration);
            services.AddReportApplicationServices(configuration);
            
            return services;
        }
    }
}