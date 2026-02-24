using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;
using Website.Application.Pagination;

namespace Website.Application.Features.CouponFeatures.Queries.GetCouponsPaged
{
    public class GetCouponsPagedQueryHandler : IRequestHandler<GetCouponsPagedQueryRequest, PagedResult<CouponListItemDto>>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ITenantProvider _tenantProvider;

        public GetCouponsPagedQueryHandler(
            ICouponRepository couponRepository,
            ITenantProvider tenantProvider)
        {
            _couponRepository = couponRepository;
            _tenantProvider = tenantProvider;
        }

        public async Task<PagedResult<CouponListItemDto>> Handle(
            GetCouponsPagedQueryRequest request,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId() ?? string.Empty;
            return await _couponRepository.GetCouponsPagedAsync(request.Filter, tenantId, cancellationToken);
        }
    }
}
