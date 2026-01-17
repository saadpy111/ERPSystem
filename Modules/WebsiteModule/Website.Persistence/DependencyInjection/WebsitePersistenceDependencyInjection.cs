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

            // Legacy Repositories (existing)
            services.AddScoped<IThemeRepository, ThemeRepository>();
            services.AddScoped<ITenantWebsiteRepository, TenantWebsiteRepository>();
            services.AddScoped<IWebsiteUnitOfWork, WebsiteUnitOfWork>();
           

            return services;
        }
    }
}

