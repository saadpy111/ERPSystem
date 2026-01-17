using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Subscription;
using Subscription.Application.Services;
using System.Reflection;

namespace Subscription.Application.DependencyInjection
{
    public static class SubscriptionApplicationDependencyInjection
    {
        public static IServiceCollection AddSubscriptionApplication(
            this IServiceCollection services)
        {



            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            // Implement SharedKernel contracts for cross-module consumption
            services.AddScoped<ISubscriptionModuleChecker, SubscriptionModuleChecker>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IPermissionModuleMapper, PermissionModuleMapper>();

            return services;
        }
    }
}
