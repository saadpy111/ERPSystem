using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.AnalyticsFeatures.Queries.GetCategoryRevenue
{
    public class GetCategoryRevenuePagedQueryHandler : IRequestHandler<GetCategoryRevenuePagedQuery, PagedResult<CategoryRevenueDto>>
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public GetCategoryRevenuePagedQueryHandler(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task<PagedResult<CategoryRevenueDto>> Handle(GetCategoryRevenuePagedQuery request, CancellationToken cancellationToken)
        {
            return await _analyticsRepository.GetCategoryRevenuePagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
