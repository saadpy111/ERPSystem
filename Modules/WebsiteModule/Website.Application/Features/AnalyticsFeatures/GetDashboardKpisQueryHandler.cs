using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.AnalyticsFeatures
{
    public class GetDashboardKpisQueryHandler : IRequestHandler<GetDashboardKpisQuery, DashboardKpiDto>
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public GetDashboardKpisQueryHandler(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task<DashboardKpiDto> Handle(GetDashboardKpisQuery request, CancellationToken cancellationToken)
        {
            // Default to 30 days if nothing provided
            int days = request.Days ?? 30;

            if (request.FromDate.HasValue || request.ToDate.HasValue)
            {
                return await _analyticsRepository.GetDashboardKpisAsync(request.FromDate, request.ToDate, cancellationToken);
            }

            return await _analyticsRepository.GetDashboardKpisAsync(null, null, cancellationToken);
        }
    }
}
