using System;

namespace Website.Application.DTOs
{
    public class CategoryRevenueDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public decimal RevenuePercentage { get; set; }
        public int ProductsCount { get; set; }
    }
}
