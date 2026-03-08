using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.AnalyticsFeatures.Queries.GetProductRevenue
{
    public class GetProductRevenuePagedQueryHandler : IRequestHandler<GetProductRevenuePagedQuery, PagedResult<ProductRevenueDto>>
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public GetProductRevenuePagedQueryHandler(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public async Task<PagedResult<ProductRevenueDto>> Handle(GetProductRevenuePagedQuery request, CancellationToken cancellationToken)
        {
            return await _analyticsRepository.GetProductRevenuePagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}
