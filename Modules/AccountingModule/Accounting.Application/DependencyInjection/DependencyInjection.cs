using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Accounting.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountingApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
            });

            services.AddAutoMapper(assembly);
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}
