using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Website.Application.Contracts.Persistence;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Persistence.Context;
using Website.Persistence.Repositories;
using Website.Persistence.Seeders;

namespace Website.Persistence.DependencyInjection
{
    public static class WebsitePersistenceDependencyInjection
    {
        public static IServiceCollection AddWebsitePersistenceDependencyInjection(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<WebsiteDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
            });

            // Generic Repository and Unit of Work
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Specialized Repositories
            services.AddScoped<IWebsiteProductRepository, WebsiteProductRepository>();
            services.AddScoped<IWebsiteCategoryRepository, WebsiteCategoryRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerAnalyticsRepository, CustomerAnalyticsRepository>();
            services.AddScoped<IWebsiteAnalyticsRepository, WebsiteAnalyticsRepository>();
            services.AddScoped<IVisitorSessionRepository,VisitorSessionRepository > ();
            services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
            services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();

            // Legacy Repositories (existing)
            services.AddScoped<IThemeRepository, ThemeRepository>();
            services.AddScoped<ITenantWebsiteRepository, TenantWebsiteRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ITestimonialRepository, TestimonialRepository>();
            services.AddScoped<INewsletterSubscriberRepository, NewsletterSubscriberRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IWebsiteUnitOfWork, WebsiteUnitOfWork>();

            return services;
        }
    }
}

