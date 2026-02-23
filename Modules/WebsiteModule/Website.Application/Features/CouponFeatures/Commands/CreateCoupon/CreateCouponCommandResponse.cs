namespace Website.Application.Features.CouponFeatures.Commands.CreateCoupon
{
    public class CreateCouponCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? CouponId { get; set; }
    }
}
