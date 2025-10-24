
using ERP.Api.DependencyInjection;
using Identity.Application.Settings;

namespace Inventory.Api
{
    public class Program
    {
        public static void Main(string[] args)
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
            app.UseCors("mypolicy");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c=>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/inventories/swagger.json", "Inventories API");
                    c.SwaggerEndpoint("/swagger/Identity/swagger.json", "Identity API");
                    c.DefaultModelExpandDepth(2);
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
