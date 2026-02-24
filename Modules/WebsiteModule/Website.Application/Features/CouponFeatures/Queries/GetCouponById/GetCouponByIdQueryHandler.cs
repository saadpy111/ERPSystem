using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Application.DTOs;

namespace Website.Application.Features.CouponFeatures.Queries.GetCouponById
{
    public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQueryRequest, CouponDetailsDto?>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ITenantProvider _tenantProvider;

        public GetCouponByIdQueryHandler(
            ICouponRepository couponRepository,
            ITenantProvider tenantProvider)
        {
            _couponRepository = couponRepository;
            _tenantProvider = tenantProvider;
        }

        public async Task<CouponDetailsDto?> Handle(
            GetCouponByIdQueryRequest request,
            CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId() ?? string.Empty;
            return await _couponRepository.GetCouponDetailsByIdAsync(request.Id, tenantId, cancellationToken);
        }
    }
}
