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

            // Register Posting Engine
            services.AddScoped<Accounting.Application.Posting.Interfaces.IPostingService, Accounting.Application.Posting.Services.PostingService>();
            services.AddTransient<Accounting.Application.Posting.Interfaces.IPostingStrategy, Accounting.Application.Posting.Strategies.SalesPostingStrategy>();
            services.AddTransient<Accounting.Application.Posting.Interfaces.IPostingStrategy, Accounting.Application.Posting.Strategies.PurchasePostingStrategy>();
            services.AddTransient<Accounting.Application.Posting.Interfaces.IPostingStrategy, Accounting.Application.Posting.Strategies.InventoryPostingStrategy>();
            services.AddTransient<Accounting.Application.Posting.Interfaces.IPostingStrategy, Accounting.Application.Posting.Strategies.ManualPostingStrategy>();
            services.AddTransient<Accounting.Application.Posting.Interfaces.IPostingStrategy, Accounting.Application.Posting.Strategies.CashPostingStrategy>();

            // Register Mapping Service
            services.AddScoped<Accounting.Application.Services.Interfaces.IAccountingMappingService, Accounting.Application.Services.Implementations.AccountingMappingService>();
            services.AddScoped<Accounting.Application.Services.Interfaces.IExchangeRateService, Accounting.Application.Services.Implementations.ExchangeRateService>();

            return services;
        }
    }
}
