using System;

namespace Website.Domain.Entities
{
    public class WebsiteAnalyticsDaily : BaseEntity
    {
        public DateTime Date { get; set; }
        public int Visitors { get; set; }
        public int UniqueVisitors { get; set; }
        public int ActiveUsers { get; set; }
        public int AddToCart { get; set; }
        public int CheckoutStarted { get; set; }
        public int OrdersCompleted { get; set; }
        public decimal Revenue { get; set; }
    }
}
