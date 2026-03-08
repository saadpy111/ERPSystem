using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.DashboardFeatures.Queries.GetDashboardOverview
{
    public class GetDashboardOverviewQueryHandler : IRequestHandler<GetDashboardOverviewQuery, DashboardOverviewDto>
    {
        private readonly IAdminDashboardRepository _dashboardRepository;

        public GetDashboardOverviewQueryHandler(IAdminDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardOverviewDto> Handle(GetDashboardOverviewQuery request, CancellationToken cancellationToken)
        {
            return await _dashboardRepository.GetDashboardOverviewAsync(request.Days, request.FromDate, request.ToDate, cancellationToken);
        }
    }
}
