using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Website.Application.DTOs;
using Website.Application.Contracts.Persistence.Repositories;

namespace Website.Application.Features.AnalyticsFeatures.Queries.GetAnalyticsDashboard
{
    public class GetAnalyticsDashboardQueryHandler : IRequestHandler<GetAnalyticsDashboardQuery, AnalyticsDashboardDto>
    {
        private readonly IWebsiteAnalyticsRepository _analyticsRepository;
        private readonly IVisitorSessionRepository _sessionRepository;

        public GetAnalyticsDashboardQueryHandler(
            IWebsiteAnalyticsRepository analyticsRepository,
            IVisitorSessionRepository sessionRepository)
        {
            _analyticsRepository = analyticsRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<AnalyticsDashboardDto> Handle(GetAnalyticsDashboardQuery request, CancellationToken cancellationToken)
        {
            var todayAnalytics = await _analyticsRepository.GetTodayAsync(cancellationToken);
            var activeSessions = await _sessionRepository.GetActiveSessionsCountAsync(cancellationToken);

            return new AnalyticsDashboardDto
            {
                VisitorsToday = todayAnalytics?.Visitors ?? 0,
                ActiveUsers = activeSessions,
                AddToCart = todayAnalytics?.AddToCart ?? 0,
                CheckoutStarted = todayAnalytics?.CheckoutStarted ?? 0,
                OrdersCompleted = todayAnalytics?.OrdersCompleted ?? 0,
                Revenue = todayAnalytics?.Revenue ?? 0m
            };
        }
    }
}
