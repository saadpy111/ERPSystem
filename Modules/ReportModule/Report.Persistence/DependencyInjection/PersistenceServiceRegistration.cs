using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Application.Contracts.Persistence.Repositories;
using Report.Persistence.Context;
using Report.Persistence.Repositories;
using Report.Persistence.Seeders;
using System;

namespace Report.Persistence.DependencyInjection
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddReportPersistenceServices(this IServiceCollection services , IConfiguration configuration)
        {
            #region DbContext
            services.AddDbContext<ReportDbContext>(options =>
            {
                var con = configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(con);
            });
            #endregion
        

            #region Register repository implementations
            services.AddScoped<IReportsRepository, ReportsRepository>();
            services.AddScoped<IReportDataSourcesRepository, ReportDataSourcesRepository>();
            services.AddScoped<IReportFieldsRepository, ReportFieldsRepository>();
            services.AddScoped<IReportParametersRepository, ReportParametersRepository>();
            services.AddScoped<IReportFiltersRepository, ReportFiltersRepository>();
            services.AddScoped<IReportGroupsRepository, ReportGroupsRepository>();
            services.AddScoped<IReportSortingsRepository, ReportSortingsRepository>();
            services.AddScoped<IEmployeeReportRepository, EmployeeReportRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            return services;
        }
    }
}