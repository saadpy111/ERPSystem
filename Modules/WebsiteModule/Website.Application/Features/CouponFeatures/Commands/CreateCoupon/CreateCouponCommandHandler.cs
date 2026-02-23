using MediatR;
using SharedKernel.Multitenancy;
using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;

namespace Website.Application.Features.CouponFeatures.Commands.CreateCoupon
{
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommandRequest, CreateCouponCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITenantProvider _tenantProvider;

        public CreateCouponCommandHandler(IUnitOfWork unitOfWork, ITenantProvider tenantProvider)
        {
            _unitOfWork = unitOfWork;
            _tenantProvider = tenantProvider;
        }

        public async Task<CreateCouponCommandResponse> Handle(CreateCouponCommandRequest request, CancellationToken cancellationToken)
        {
            var tenantId = _tenantProvider.GetTenantId() ?? string.Empty;

            var coupon = new Coupon
            {
                Code = request.Code.ToUpperInvariant(),
                Name = request.Name,
                DiscountType = request.DiscountType,
                DiscountValue = request.DiscountValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = request.IsActive,
                CanBeCombinedWithOffers = request.CanBeCombinedWithOffers,
                UsageLimit = request.UsageLimit,
                UsagePerUserLimit = request.UsagePerUserLimit,
                MinimumOrderAmount = request.MinimumOrderAmount,
                TenantId = tenantId
            };

            await _unitOfWork.Repository<Coupon>().AddAsync(coupon);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateCouponCommandResponse
            {
                Success = true,
                CouponId = coupon.Id,
                Message = "Coupon created successfully."
            };
        }
    }
}
