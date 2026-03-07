namespace Website.Application.DTOs
{
    public class AnalyticsDashboardDto
    {
        public int VisitorsToday { get; set; }
        public int ActiveUsers { get; set; }
        public int AddToCart { get; set; }
        public int CheckoutStarted { get; set; }
        public int OrdersCompleted { get; set; }
        public decimal Revenue { get; set; }
    }
}
