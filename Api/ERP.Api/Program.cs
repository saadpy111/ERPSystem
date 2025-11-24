using ERP.Api.DependencyInjection;
using Identity.Application.Settings;
using Report.Persistence.Context;
using Report.Persistence.Seeders;
using System;

namespace Inventory.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            #region appsetting
            //builder.Configuration
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.AddJsonFile("appsettingsIdentity.json", optional: true, reloadOnChange: true)
         


            //.AddEnvironmentVariables();
            #endregion

            builder.Services.AddApiDependencyInjection(builder.Configuration);


            var app = builder.Build();
            //using (var scope = app.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
            //    var seeder = new ReportSeeder(db);
            //    await seeder.SeedAsync();
            //}

            app.UseCors("mypolicy");
       
                app.UseSwagger();

                app.UseSwaggerUI(c=>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/inventories/swagger.json", "Inventories API");
                    c.SwaggerEndpoint("/swagger/Identity/swagger.json", "Identity API");
                    c.SwaggerEndpoint("/swagger/procurement/swagger.json", "Procurement API");
                    c.SwaggerEndpoint("/swagger/Hr/swagger.json", "Hr API");
                    c.DefaultModelExpandDepth(2);
                });

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}