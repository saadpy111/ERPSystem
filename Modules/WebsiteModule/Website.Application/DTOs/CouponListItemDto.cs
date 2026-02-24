using Website.Domain.Enums;

namespace Website.Application.DTOs
{
    public class CouponListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UsageCount { get; set; }
        public int? UsageLimit { get; set; }
        public double? UsagePercentage { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
    }
}
