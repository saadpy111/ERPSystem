
using Inventory.Api.DependencyInjection;

namespace Inventory.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddApiDependencyInjection(builder.Configuration);

       
           //.AddJsonOptions(options =>
           //{
           //    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
           //});

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            var app = builder.Build();
            app.UseCors("mypolicy");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(c=>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/inventories/swagger.json", "Inventories API");
                    c.DefaultModelExpandDepth(2);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
