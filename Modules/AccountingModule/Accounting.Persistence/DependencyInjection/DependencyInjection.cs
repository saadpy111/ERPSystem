using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Accounting.Persistence.Context;

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

            // Uncomment the following registrations once the corresponding interfaces 
            // and repository implementations are created.

            // services.AddScoped<IUnitOfWork, UnitOfWork>();
            // services.AddScoped<IAccountRepository, AccountRepository>();
            // services.AddScoped<IJournalEntryRepository, JournalEntryRepository>();
            // services.AddScoped<IPartnerRepository, PartnerRepository>();
            // services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            // services.AddScoped<IVoucherRepository, VoucherRepository>();

            return services;
        }
    }
}
