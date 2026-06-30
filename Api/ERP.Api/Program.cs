using ERP.Api.DependencyInjection;
using Identity.Domain.Entities;
using Identity.Persistense.Context;
using Identity.Persistense.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Report.Persistence.Seeders;
using SharedKernel.Middleware;
using Subscription.Persistence.Context;
using Subscription.Persistence.Seeders;
using Website.Api.Middleware;
using Website.Persistence.Context;
using Website.Persistence.Seeders;

namespace Inventory.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApiDependencyInjection(builder.Configuration);

            var app = builder.Build();

            // Seed data on startup
            using (var scope = app.Services.CreateScope())
            {
                // Seed Identity module (tenants, permissions, roles, users)
                var identityContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

   


                //var permissionSeeder = new PermissionSeeder(identityContext);
                //await permissionSeeder.SeedAsync();

    

                // Seed Subscription module (modules and plans)
                var subscriptionContext = scope.ServiceProvider.GetRequiredService<SubscriptionDbContext>();
                var moduleSeeder = new ModuleSeeder(subscriptionContext);
                await moduleSeeder.SeedAsync();

                var planSeeder = new SubscriptionPlanSeeder(subscriptionContext);
                await planSeeder.SeedAsync();

                // Seed report module
                var employeereportseeder = scope.ServiceProvider.GetRequiredService<EmployeeReportSeedService>();
                var inventoryreportseeder = scope.ServiceProvider.GetRequiredService<InventoryReportSeedService>();
                await employeereportseeder.SeedAsync();
                await inventoryreportseeder.SeedAsync();

                //themes 
                var websiteDbContext = scope.ServiceProvider.GetRequiredService<WebsiteDbContext>();
                await ThemeSeeder.SeedAsync(websiteDbContext);


            }

            app.UseCors("mypolicy");
       
            app.UseSwagger();

            app.UseSwaggerUI(c=>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/inventories/swagger.json", "Inventories API");
                c.SwaggerEndpoint("/swagger/Identity/swagger.json", "Identity API");
                c.SwaggerEndpoint("/swagger/procurement/swagger.json", "Procurement API");
                c.SwaggerEndpoint("/swagger/Hr/swagger.json", "Hr API");
                c.SwaggerEndpoint("/swagger/Report/swagger.json", "Report API");
                c.SwaggerEndpoint("/swagger/Subscription/swagger.json", "Subscription API");
                c.SwaggerEndpoint("/swagger/Website/swagger.json", "Website API");
                c.SwaggerEndpoint("/swagger/Accounting/swagger.json", "Accounting API");
                c.DefaultModelExpandDepth(2);
            });

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            
            // CRITICAL: Middleware order for multi-tenancy + authorization
            app.UseAuthentication();                         // 1. Authenticate user (JWT validation)
            app.UseMiddleware<TenantResolutionMiddleware>(); // 2. Extract tenant from JWT
            app.UseMiddleware<VisitorTrackingMiddleware>(); // 3. Track visitor session
            //app.UseMiddleware<TenantRequiredMiddleware>();
            app.UseAuthorization();                          // 3. Check permissions (with tenant context)

            app.MapControllers();

            app.Run();
        }
    }
}
