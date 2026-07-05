using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Subscription.Application.DependencyInjection;
using Subscription.Infrastructure.Payment.Extensions;
using Subscription.Persistence.DependencyInjection;

namespace Subscription.Api.DependencyInjection
{
    public static class SubscriptionApiDependencyInjection
    {
        public static IServiceCollection AddSubscriptionApiDependencyInjection(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Add all Subscription module layers
            services.AddSubscriptionPersistence(configuration);
            services.AddSubscriptionApplication();
            services.AddPaymobPayments(configuration);

            return services;
        }
    }
}
