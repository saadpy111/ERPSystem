using Hr.Api.Controllers;
using Hr.Api.DependencyInjection;
using Hr.Infrastructure.FileService;
using Identity.Api.Controllers;
using Identity.Api.DependencyInjection;
using Inventory.Api.Controllers;
using Inventory.Api.DependencyInjection;
using Inventory.Application.Contracts.Infrastruture.FileService;
using Inventory.Infrastructure.FileService;
using Microsoft.AspNetCore.Mvc;
using Procurement.Api.Controllers;
using Procurement.Api.DependencyInjection;
using Procurement.Infrastructure.FileService;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ERP.Api.DependencyInjection
{
    public static class ApiDependencyInjection
    {

        
        public static IServiceCollection AddApiDependencyInjection(this IServiceCollection services , IConfiguration configuration)
        {
            #region ModulesDI
            services.AddInventoryApiDependencyInjection(configuration);

            services.AddIdentityApiDependencyInjection(configuration);
            
            services.AddProcurementApiDependencyInjection(configuration);

            services.AddHrApiDependencyInjection(configuration);

            #endregion

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

                 options.SwaggerDoc("Hr", new() { Title = "HR API", Version = "v1" });
                 options.SwaggerDoc("inventories", new() { Title = "Inventories API", Version = "v1" });
                 options.SwaggerDoc("Identity", new() { Title = "Identity API", Version = "v1" });
                 options.SwaggerDoc("procurement", new() { Title = "Procurement API", Version = "v1" });
                 options.DocInclusionPredicate((docName, apiDesc) =>
                 {
                     if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                     var groupName = methodInfo.DeclaringType?
                         .GetCustomAttributes(true)
                         .OfType<ApiExplorerSettingsAttribute>()
                         .FirstOrDefault()?.GroupName;

                     return groupName == docName;
                 });
                 options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                 //options.UseInlineDefinitionsForEnums();
                 //options.SchemaGeneratorOptions.SchemaIdSelector = type => type.FullName;
             });

            #endregion

            #region File
            services.AddScoped<IFileService>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                return new LocalFileService(env.WebRootPath);
            });
            services.AddScoped<Hr.Application.Contracts.Infrastructure.FileService.IFileService>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                return new HrLocalFileService(env.WebRootPath);
            });
            services.AddScoped<Procurement.Application.Contracts.Infrastructure.FileService.IProcurementFileService>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                return new ProcurementLocalFileService(env.WebRootPath);
            });
            #endregion

            services.AddControllers().AddApplicationPart(typeof(LocationController).Assembly);
            services.AddControllers().AddApplicationPart(typeof(AuthController).Assembly);
            services.AddControllers().AddApplicationPart(typeof(VendorsController).Assembly);
            services.AddControllers().AddApplicationPart(typeof(EmployeesController).Assembly);
            return services;
        }
    }
}