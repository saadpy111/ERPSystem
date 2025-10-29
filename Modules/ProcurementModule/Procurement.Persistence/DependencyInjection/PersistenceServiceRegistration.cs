using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Procurement.Application.Contracts.Persistence.Repositories;
using Procurement.Persistence.Context;
using Procurement.Persistence.Repositories;

namespace Procurement.Persistence.DependencyInjection
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region DbContext
            services.AddDbContext<ProcurementDbContext>((serviceProvider, options) =>
            {
                var con = configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(con);
             
            });
            #endregion
            // Register repositories
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddScoped<IPurchaseOrderItemRepository, PurchaseOrderItemRepository>();
            services.AddScoped<IGoodsReceiptRepository, GoodsReceiptRepository>();
            services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
            services.AddScoped<IPurchaseRequisitionRepository, PurchaseRequisitionRepository>();
            
            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}