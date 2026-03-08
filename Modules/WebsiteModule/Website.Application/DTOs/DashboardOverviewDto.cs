namespace Website.Application.DTOs
{
    public class DashboardOverviewDto
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public int TotalVisitors { get; set; }
        public int ActiveOffers { get; set; }
        public int PendingOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
