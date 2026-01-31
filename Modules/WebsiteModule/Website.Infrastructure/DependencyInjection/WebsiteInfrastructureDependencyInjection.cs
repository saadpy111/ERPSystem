using Microsoft.Extensions.DependencyInjection;
using Website.Application.Contracts.Infrastruture.FileService;
using Website.Infrastructure.FileService;

namespace Website.Infrastructure.DependencyInjection
{
    public static class WebsiteInfrastructureDependencyInjection
    {
        public static IServiceCollection AddWebsiteInfrastructureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IFileService, WebsiteLocalFileService>();
            
            return services;
        }
    }
}
