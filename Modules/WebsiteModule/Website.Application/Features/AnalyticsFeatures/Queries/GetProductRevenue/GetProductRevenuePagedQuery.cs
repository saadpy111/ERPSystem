using MediatR;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.AnalyticsFeatures.Queries.GetProductRevenue
{
    public class GetProductRevenuePagedQuery : IRequest<PagedResult<ProductRevenueDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
