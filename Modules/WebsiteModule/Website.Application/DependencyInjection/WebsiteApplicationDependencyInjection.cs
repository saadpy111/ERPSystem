using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Website;
using Website.Application.Services;
using Website.Application.Features.StorefrontFeatures.Services;
using System.Reflection;

namespace Website.Application.DependencyInjection
{
    public static class WebsiteApplicationDependencyInjection
    {
        public static IServiceCollection AddWebsiteApplicationDependencyInjection(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // MediatR handlers
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Application services
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<IProductPricingService, ProductPricingService>();

            // Cross-module services (consumed by IdentityModule)
            services.AddScoped<IWebsiteProvisioningService, WebsiteProvisioningService>();
            services.AddScoped<ITenantDomainResolver, TenantDomainResolver>();

            return services;
        }
    }
}
