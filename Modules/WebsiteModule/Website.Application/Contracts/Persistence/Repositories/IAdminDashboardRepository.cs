using System;
using System.Threading;
using System.Threading.Tasks;
using Website.Application.DTOs;
using System.Collections.Generic;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface IAdminDashboardRepository
    {
        Task<DashboardOverviewDto> GetDashboardOverviewAsync(int? days, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
        Task<List<WeeklyAnalyticsDto>> GetWeeklyOverviewAsync(int? days, CancellationToken cancellationToken = default);
    }
}
