using Website.Application.Contracts.Persistence.Repositories;
using Website.Domain.Entities;
using Website.Domain.Enums;

namespace Website.Application.Services
{
    public class CouponValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public Coupon? Coupon { get; set; }
    }

    public interface ICouponService
    {
        /// <summary>
        /// Validates a coupon code against business rules (active, dates, usage limits, minimum amount).
        /// </summary>
        Task<CouponValidationResult> ValidateCouponAsync(
            string code,
            string userId,
            decimal orderSubtotal,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Calculates the discount amount for a validated coupon.
        /// Ensures discount never exceeds the order total.
        /// </summary>
        decimal CalculateCouponDiscount(Coupon coupon, decimal orderTotal);
    }

    public class CouponService : ICouponService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CouponService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CouponValidationResult> ValidateCouponAsync(
            string code,
            string userId,
            decimal orderSubtotal,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return new CouponValidationResult { IsValid = false, Message = "Coupon code is empty." };
            }

            var coupon = await _unitOfWork.Repository<Coupon>().GetFirstAsync(
                c => c.Code == code.ToUpperInvariant(),
                asNoTracking: true,
                c => c.Usages);

            if (coupon == null)
            {
                return new CouponValidationResult { IsValid = false, Message = "Coupon code not found." };
            }

            if (!coupon.IsActive)
            {
                return new CouponValidationResult { IsValid = false, Message = "Coupon is inactive." };
            }

            var now = DateTime.UtcNow;
            if (now < coupon.StartDate || now > coupon.EndDate)
            {
                return new CouponValidationResult { IsValid = false, Message = "Coupon has expired or is not yet active." };
            }

            if (coupon.MinimumOrderAmount.HasValue && orderSubtotal < coupon.MinimumOrderAmount.Value)
            {
                return new CouponValidationResult 
                { 
                    IsValid = false, 
                    Message = $"Order subtotal must be at least {coupon.MinimumOrderAmount.Value:C} to use this coupon." 
                };
            }

            // Check total usage limit
            if (coupon.UsageLimit.HasValue)
            {
                var totalUsageCount = coupon.Usages.Count;
                if (totalUsageCount >= coupon.UsageLimit.Value)
                {
                    return new CouponValidationResult { IsValid = false, Message = "Coupon usage limit reached." };
                }
            }

            // Check per-user usage limit
            if (coupon.UsagePerUserLimit.HasValue)
            {
                var userUsageCount = coupon.Usages.Count(u => u.UserId == userId);
                if (userUsageCount >= coupon.UsagePerUserLimit.Value)
                {
                    return new CouponValidationResult { IsValid = false, Message = "You have reached the usage limit for this coupon." };
                }
            }

            return new CouponValidationResult { IsValid = true, Coupon = coupon };
        }

        public decimal CalculateCouponDiscount(Coupon coupon, decimal orderTotal)
        {
            decimal discount = 0;

            if (coupon.DiscountType == DiscountType.Percentage)
            {
                discount = orderTotal * (coupon.DiscountValue / 100m);
            }
            else if (coupon.DiscountType == DiscountType.Fixed)
            {
                discount = coupon.DiscountValue;
            }

            // Clean separation: Ensures discount never negative and never exceeds total
            discount = Math.Max(0, discount);
            return Math.Min(discount, orderTotal);
        }
    }
}
