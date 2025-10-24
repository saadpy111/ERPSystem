using Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Factory;
using Inventory.Application.Helpers.Strategies.StockMoveFactoryHandler.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.DependencyInjection
{
    public static class InventoryApplicationDependencyInjection
    {
        public static IServiceCollection AddInventoryApplicationDependencyInjection(this IServiceCollection services , IConfiguration configuration)
        {

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            #region StockQuantHandler
              services.AddScoped<PurchaseStockMoveHandler>();
              services.AddScoped<TransferStockMoveHandler>();
              services.AddScoped<AdjustmentStockMoveHandler>();
              services.AddScoped<ReturnStockMoveHandler>();
              services.AddScoped<UnderReviewStockMoveHandler>();
              services.AddScoped<SalesStockMoveHandler>();
              services.AddScoped<IStockMoveHandlerFactory, StockMoveHandlerFactory>();
            #endregion

            

            return services;
        }
    }
}
