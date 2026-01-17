using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Subscription.Application.Contracts.Persistence;
using Subscription.Persistence.Context;
using Subscription.Persistence.Repositories;
using Subscription.Persistence.Seeders;

namespace Subscription.Persistence.DependencyInjection
{
    public static class SubscriptionPersistenceDependencyInjection
    {
        public static IServiceCollection AddSubscriptionPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<SubscriptionDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ConnectionString"),
                    b => b.MigrationsAssembly(typeof(SubscriptionDbContext).Assembly.FullName)));

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Seeder
            services.AddScoped<SubscriptionPlanSeeder>();
            // Repositories
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<IPlanModuleRepository, PlanModuleRepository>();
            services.AddScoped<ITenantSubscriptionRepository, TenantSubscriptionRepository>();

            return services;
        }
    }
}
