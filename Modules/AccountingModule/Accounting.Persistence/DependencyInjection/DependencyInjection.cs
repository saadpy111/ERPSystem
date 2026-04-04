using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Accounting.Persistence.Context;
using Accounting.Application.Interfaces.Repositories;
using Accounting.Persistence.Common;
using Accounting.Persistence.Repositories.Implementations;

namespace Accounting.Persistence.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAccountingPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {


            #region DbContext
            services.AddDbContext<AccountingDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ConnectionString"),
                    builder => builder.MigrationsAssembly(typeof(AccountingDbContext).Assembly.FullName));
            });
            #endregion

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IJournalEntryRepository, JournalEntryRepository>();
            services.AddScoped<IPartnerRepository, PartnerRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IAccountingMappingRepository, AccountingMappingRepository>();

            services.AddScoped<Accounting.Application.Interfaces.Contexts.IAccountingDbContext>(provider => provider.GetRequiredService<AccountingDbContext>());

            return services;
        }
    }
}
