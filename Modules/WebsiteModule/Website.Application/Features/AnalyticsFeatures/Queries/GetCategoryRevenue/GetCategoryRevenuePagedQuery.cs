using MediatR;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.AnalyticsFeatures.Queries.GetCategoryRevenue
{
    public class GetCategoryRevenuePagedQuery : IRequest<PagedResult<CategoryRevenueDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
