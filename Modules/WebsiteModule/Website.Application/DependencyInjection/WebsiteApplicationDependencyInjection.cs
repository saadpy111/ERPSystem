using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Website;
using Website.Application.Mappers;
using Website.Application.Services;
using Website.Application.Features.StorefrontFeatures.Services;
using System.Reflection;
using SharedKernel.Contracts;

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

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Application services
            services.AddScoped<IPricingService, PricingService>();
            services.AddScoped<IOfferEligibilityService, OfferEligibilityService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IProductPricingService, ProductPricingService>();

            // Mappers
            services.AddScoped<IWebsiteConfigMapper, WebsiteConfigMapper>();

            // Cross-module services (consumed by IdentityModule)
            services.AddScoped<IWebsiteProvisioningService, WebsiteProvisioningService>();
            services.AddScoped<IWebsiteImageService, WebsiteImageService>();
            services.AddScoped<ITenantDomainResolver, TenantDomainResolver>();

            return services;
        }
    }
}
