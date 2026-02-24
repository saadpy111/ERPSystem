using MediatR;
using Website.Application.DTOs;

namespace Website.Application.Features.OrderFeatures.Queries.GetAdminDashboardStats
{
    public class GetAdminDashboardStatsQuery : IRequest<AdminDashboardStatsDto>
    {
    }
}
