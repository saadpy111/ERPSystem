using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface IOrderRepository
    {
        Task<PagedResult<AdminOrderListDto>> GetAdminOrdersPagedAsync(
            AdminOrderFilter filter,
           
            CancellationToken cancellationToken);

        Task<OrderDetailsDto?> GetAdminOrderDetailsAsync(
            Guid orderId,
           
            CancellationToken cancellationToken);

        Task<PagedResult<UserOrderListDto>> GetUserOrdersPagedAsync(
            string userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);

        Task<UserOrderDetailsDto?> GetUserOrderDetailsAsync(
            Guid orderId,
            string userId,
            CancellationToken cancellationToken);

        Task<AdminDashboardStatsDto> GetAdminDashboardStatsAsync(
            CancellationToken cancellationToken);
    }
}
