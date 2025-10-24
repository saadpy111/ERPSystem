

using Inventory.Application.Contracts.Persistence.Repositories;
using Inventory.Persistence.Context;
using Inventory.Persistence.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Persistence.DependencyInjection
{
    public static  class InventoryPersistenceDependencyInjection
    {
        public static IServiceCollection AddInventoryPersistenceDependencyInjection(this IServiceCollection services ,IConfiguration configuration)
        {


            #region DbContext
            services.AddDbContext<InventoryDbContext>(options =>
            {
                var con = configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(con);
            });
            #endregion

            #region UOW
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion
            return services;
        }

    }
}
