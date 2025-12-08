using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Application.Contracts.Persistence.Repositories;
using Report.Application.Services;
using System.Reflection;
using System.Net.Http.Headers;

namespace Report.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddReportApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Add AutoMapper
            services.AddAutoMapper(typeof(ApplicationServiceRegistration));

            services.AddHttpClient<AIQueryBuilderService>(client =>
            {
                client.BaseAddress = new Uri("https://api.groq.com/openai/v1/");

                var apiKey = configuration["Groq:ApiKey"];

                if (!string.IsNullOrEmpty(apiKey))
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", apiKey);
                }
            });

            return services;
        }
    }
}