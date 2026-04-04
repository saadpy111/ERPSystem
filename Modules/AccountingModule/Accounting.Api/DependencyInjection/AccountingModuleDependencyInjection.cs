using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Accounting.Application.DependencyInjection;
using Accounting.Domain.DependencyInjection;
using Accounting.Persistence.DependencyInjection;

namespace Accounting.Api.DependencyInjection
{
    public static class AccountingModuleDependencyInjection
    {
        public static IServiceCollection AddAccountingModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Domain Layer (if any needed later)
            services.AddAccountingDomainServices();

            // Register Application Layer
            services.AddAccountingApplicationServices();
            
            // Register Persistence Layer
            services.AddAccountingPersistenceServices(configuration);
            
            // Register API Layer
            services.AddAccountingApiServices();

            return services;
        }
    }
}
