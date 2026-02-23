using MediatR;
using Website.Domain.Enums;

namespace Website.Application.Features.CouponFeatures.Commands.CreateCoupon
{
    public class CreateCouponCommandRequest : IRequest<CreateCouponCommandResponse>
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool CanBeCombinedWithOffers { get; set; }
        public int? UsageLimit { get; set; }
        public int? UsagePerUserLimit { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
    }
}
