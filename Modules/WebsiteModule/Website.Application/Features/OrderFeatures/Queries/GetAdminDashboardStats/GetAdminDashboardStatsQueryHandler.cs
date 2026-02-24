using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.OrderFeatures.Queries.GetAdminDashboardStats
{
    public class GetAdminDashboardStatsQueryHandler : IRequestHandler<GetAdminDashboardStatsQuery, AdminDashboardStatsDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAdminDashboardStatsQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<AdminDashboardStatsDto> Handle(
            GetAdminDashboardStatsQuery request, 
            CancellationToken cancellationToken)
        {
            return await _orderRepository.GetAdminDashboardStatsAsync(cancellationToken);
        }
    }
}
