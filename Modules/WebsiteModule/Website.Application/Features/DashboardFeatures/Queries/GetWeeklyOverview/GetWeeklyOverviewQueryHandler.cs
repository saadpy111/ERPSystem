using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.DashboardFeatures.Queries.GetWeeklyOverview
{
    public class GetWeeklyOverviewQueryHandler : IRequestHandler<GetWeeklyOverviewQuery, List<WeeklyAnalyticsDto>>
    {
        private readonly IAdminDashboardRepository _dashboardRepository;

        public GetWeeklyOverviewQueryHandler(IAdminDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<List<WeeklyAnalyticsDto>> Handle(GetWeeklyOverviewQuery request, CancellationToken cancellationToken)
        {
            return await _dashboardRepository.GetWeeklyOverviewAsync(request.Days, cancellationToken);
        }
    }
}
