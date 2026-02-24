using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Contracts.Persistence.Repositories
{
    public interface ICouponRepository
    {
        Task<PagedResult<CouponListItemDto>> GetCouponsPagedAsync(
            CouponFilter filter,
            string tenantId,
            CancellationToken cancellationToken = default);

        Task<CouponDetailsDto?> GetCouponDetailsByIdAsync(
            Guid couponId,
            string tenantId,
            CancellationToken cancellationToken = default);
    }
}
