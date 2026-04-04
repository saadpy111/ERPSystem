using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Domain.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountingDomainServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
