using System.Threading;
using System.Threading.Tasks;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface IAnalyticsRepository
    {
        Task<PagedResult<ProductRevenueDto>> GetProductRevenuePagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<PagedResult<CategoryRevenueDto>> GetCategoryRevenuePagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task<DashboardKpiDto> GetDashboardKpisAsync(
                DateTime? fromDate,
                DateTime? toDate,
                CancellationToken cancellationToken = default);
    }
}
