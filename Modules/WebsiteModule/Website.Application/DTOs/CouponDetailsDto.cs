using Website.Domain.Enums;

namespace Website.Application.DTOs
{
    public class CouponDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysRemaining { get; set; }
        public string Status { get; set; } = string.Empty;

        // Statistics
        public int UsageCount { get; set; }
        public int? UsageLimit { get; set; }
        public int? UsagePerUserLimit { get; set; }
        public double? UsagePercentage { get; set; }
        public int OrdersCount { get; set; }
        public int UniqueUsersCount { get; set; }
        public decimal TotalDiscountAmountGiven { get; set; }
        public decimal TotalRevenueGenerated { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
