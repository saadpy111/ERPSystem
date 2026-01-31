using Hr.Api.Controllers;
using Hr.Api.DependencyInjection;
using Hr.Infrastructure.FileService;
using Identity.Api.Controllers;
using Identity.Api.DependencyInjection;
using Inventory.Api.Controllers;
using Inventory.Api.DependencyInjection;
using Inventory.Application.Contracts.Infrastruture.FileService;
using Inventory.Infrastructure.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Procurement.Api.Controllers;
using Procurement.Api.DependencyInjection;
using Procurement.Infrastructure.FileService;
using Report.Api.Controllers;
using Report.Api.DependencyInjection;
using SharedKernel.Authorization;
using SharedKernel.Multitenancy;
using Subscription.Api.DependencyInjection;
using Subscription.Api.Controllers;
using Swashbuckle.AspNetCore.SwaggerGen;
using Website.Api.Controllers;
using Website.Api.DependencyInjection;
using Website.Infrastructure.FileService;

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
        
            services.AddReportApiServices(configuration);
            
            services.AddSubscriptionApiDependencyInjection(configuration);
            
            services.AddWebsiteApiDependencyInjection(configuration);
            
            #endregion

            #region Multi-Tenancy & Authorization
            // Register tenant provider
            services.AddScoped<ITenantProvider, TenantProvider>();

            // Register authorization infrastructure
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
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
                 options.SwaggerDoc("Report", new() { Title = "Report API", Version = "v1" });
                 options.SwaggerDoc("Subscription", new() { Title = "Subscription API", Version = "v1" });
                 options.SwaggerDoc("Website", new() { Title = "Website API", Version = "v1" });
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
            services.AddScoped<Website.Application.Contracts.Infrastruture.FileService.IFileService>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                return new WebsiteLocalFileService(env);
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
            services.AddControllers().AddApplicationPart(typeof(ReportsController).Assembly);
            services.AddControllers().AddApplicationPart(typeof(PlansController).Assembly);
            services.AddControllers().AddApplicationPart(typeof(ThemesController).Assembly);
            return services;
        }
    }
}