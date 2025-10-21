using Inventory.Api.Controllers;
using Inventory.Application.Contracts.Infrastruture.FileService;
using Inventory.Application.DependencyInjection;
using Inventory.Infrastructure.DependencyInjection;
using Inventory.Infrastructure.FileService;
using Inventory.Persistence.DependencyInjection;

namespace Inventory.Api.DependencyInjection
{
    public static class ApiDependencyInjection
    {
        
        public static IServiceCollection AddApiDependencyInjection(this IServiceCollection services , IConfiguration configuration)
        {

            services.AddInventoryApiDependencyInjection(configuration);




            #region Cors
            services.AddCors(options =>
            {
                options.AddPolicy("mypolicy", policy =>
                {
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                });
            });
            #endregion
            #region Swagger
            services.AddSwaggerGen(options =>
             {
                options.UseInlineDefinitionsForEnums();
                options.SchemaGeneratorOptions.SchemaIdSelector = type => type.FullName;
             });

            #endregion
            #region File
            services.AddScoped<IFileService>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                return new LocalFileService(env.WebRootPath);
            });
            #endregion
            //services.AddControllers().AddApplicationPart(typeof(LocationController).Assembly);

            return services;
        }
    }
}
