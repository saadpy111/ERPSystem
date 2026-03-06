namespace Website.Application.DTOs
{
    public class CustomerListDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public int OrdersCount { get; set; }
        public string? PhoneNumber { get; set; }

        public decimal TotalSpent { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public DateTime CustomerSince { get; set; }
    }

    public class CustomerDetailsDto
    {
        // Profile Info
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CustomerSince { get; set; }

        // Purchase Stats
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public DateTime? FirstOrderDate { get; set; }
        public string? MostPurchasedCategory { get; set; }

        // Customer Behavior
        public double AverageDaysBetweenOrders { get; set; }
        public string? FavoritePurchaseDay { get; set; }
        public double ReturnRate { get; set; }
        public int TotalItemsPurchased { get; set; }
        public int ReturnedItems { get; set; }

        // Recent Orders (paginated via separate query in handler)
        public List<CustomerOrderSummaryDto> RecentOrders { get; set; } = new();
        public int RecentOrdersTotalCount { get; set; }
    }

    public class CustomerOrderSummaryDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int ItemsCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
