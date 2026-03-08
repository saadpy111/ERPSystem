using System;

namespace Website.Application.DTOs
{
    public class ProductRevenueDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int OrdersCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RevenuePercentage { get; set; }
    }
}
