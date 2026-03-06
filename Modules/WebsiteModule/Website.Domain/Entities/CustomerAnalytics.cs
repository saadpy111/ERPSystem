using System;

namespace Website.Domain.Entities
{
    /// <summary>
    /// Aggregated purchase statistics for a customer.
    /// Used for performance optimization of admin customer lists and details.
    /// </summary>
    public class CustomerAnalytics : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public int OrdersCount { get; set; }

        public decimal TotalSpent { get; set; }

        public decimal AverageOrderValue { get; set; }

        public DateTime? LastOrderDate { get; set; }

        public double AverageDaysBetweenOrders { get; set; }

        public string? FavoritePurchaseDay { get; set; }

        public int TotalItemsPurchased { get; set; }

        public int ReturnedItems { get; set; }

        public double ReturnRate { get; set; }

        public DateTime? FirstOrderDate { get; set; }

        public string? MostPurchasedCategory { get; set; }
    }
}
